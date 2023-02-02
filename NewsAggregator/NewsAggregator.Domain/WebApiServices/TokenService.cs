using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregator.Core.Interfaces.WebApiInterfaces;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using NewsAggregetor.CQS.Models.Queries.TokenQueries;


namespace NewsAggregator.Domain.WebApiServices
{
    public class TokenService : ITokenService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TokenService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountServiceCQS _accountServiceCQS;
        private readonly IJwtService _jwtService;
        private readonly IMediator _mediator;


        public TokenService(IMapper mapper,
            ILogger<TokenService> logger,
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IAccountServiceCQS accountServiceCQS, 
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jwtService = jwtService;
            _mapper = mapper;
            _accountServiceCQS = accountServiceCQS;
            _mediator = mediator;
        }

        public async Task<JwtAuthDto> GetToken(LoginDto request, string ipAddress)
        {
            try
            {
                var user = await _accountServiceCQS.GetUserByEmailAsync(request.Login);
                if (!await _accountServiceCQS.CheckPasswordByEmailAsync(request.Login, request.Password))
                {
                    _logger.LogWarning("Incorrect password");
                    return null;
                }

                if (user == null)
                {
                    _logger.LogWarning("This login is doesn't exists");
                    return null;
                }

                var jwtToken = _jwtService.GenerateJwtToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken(ipAddress);
                refreshToken.UserId = user.Id;
                await _unitOfWork.RefreshTokens.Add(_mapper.Map<RefreshToken>(refreshToken));
                await _unitOfWork.Save();

                return new JwtAuthDto(user, jwtToken, refreshToken.Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            } 
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            try
            {
                var refreshToken = await _mediator.Send(new GetRefreshTokenQuery(token), 
                    new CancellationToken());

                if (refreshToken == null || !refreshToken.IsActive)
                    throw new ArgumentException("Invalid token", "token");

                await RevokeRefreshToken(refreshToken, ipAddress, $"Revoke wthout replacement : {token}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            } 
        }

        public async Task<JwtAuthDto> RefreshToken(string? token, string ipAddress)
        {
            try
            {
                var user = await _accountServiceCQS.GetUserByRefreshTokenAsync(token);

                var refreshToken = await _mediator.Send(new GetRefreshTokenQuery(token),
                    new CancellationToken());


                if (refreshToken == null || !refreshToken.IsActive)
                    throw new ArgumentException("Invalid token", "token");

                if (refreshToken.IsRevoked)
                {
                    await RevokeDescendantRefreshToken(refreshToken, ipAddress,
                        $"Attempted reuse of revoked ancestor token: {token}");
                }

                var refreshTokenDto = await RotateRefreshToken(refreshToken, ipAddress);
                refreshTokenDto.UserId = user.Id;

                await _unitOfWork.RefreshTokens.Add(_mapper.Map<RefreshToken>(refreshTokenDto));
                await _unitOfWork.Save();

                await RemoveOldRefreshTokens(user);

                var jwtToken = _jwtService.GenerateJwtToken(user);

                return new JwtAuthDto(user, jwtToken, refreshTokenDto.Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        private async Task RevokeDescendantRefreshToken(RefreshToken token, string ipAddress, string reason)
        {
            try
            {
                if (!string.IsNullOrEmpty(token.ReplacedByToken))
                {
                    var childToken = await _mediator.Send(new GetChildTokenQuery(token.ReplacedByToken),
                        new CancellationToken());

                    if (childToken.IsActive)
                    {
                        await RevokeRefreshToken(childToken, ipAddress, reason);
                    }
                    else
                    {
                        await RevokeDescendantRefreshToken(childToken, ipAddress, reason);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        private async Task RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            try
            {
                await _unitOfWork.RefreshTokens.PatchAsync(token.Id, new List<PatchModel>
                {
                    new PatchModel() { PropertyName = Variables.RefreshTokens.Revoked, PropertyValue = DateTime.UtcNow },
                    new PatchModel() { PropertyName = Variables.RefreshTokens.RevokedByIp, PropertyValue = ipAddress },
                    new PatchModel() { PropertyName = Variables.RefreshTokens.ReasonOfRevoke, PropertyValue = reason },
                    new PatchModel() { PropertyName = Variables.RefreshTokens.ReplacedByToken, PropertyValue = replacedByToken },
                });
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        private async Task RemoveOldRefreshTokens(UserDto userDto)
        {
            try
            {
                await _unitOfWork.RefreshTokens.RemoveRange(token => !token.IsActive && token.UserId.Equals(userDto.Id));
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        private async Task<RefreshTokenDto> RotateRefreshToken(RefreshToken token, string ipAddress)
        {
            try
            {
                var newRefreshToken = _jwtService.GenerateRefreshToken(ipAddress);
                await RevokeRefreshToken(token, ipAddress, "Replaced by new token", newRefreshToken.Token);
                return newRefreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}
