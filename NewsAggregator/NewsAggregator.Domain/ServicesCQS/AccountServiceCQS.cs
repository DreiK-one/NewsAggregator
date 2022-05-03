using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using System.Text;
using System.Security.Cryptography;
using NewsAggregetor.CQS.Models.Commands.AccountCommands;

namespace NewsAggregator.Domain.ServicesCQS
{
    public class AccountServiceCQS : IAccountServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CommentServiceCQS> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public AccountServiceCQS(IMapper mapper,
            ILogger<CommentServiceCQS> logger,
            IConfiguration configuration,
            IMediator mediator)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetUserByIdQuery(id),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            try
            {
                var upperEmail = email.ToUpperInvariant();

                return await _mediator.Send(new GetUserByEmailQuery(upperEmail),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            } 
        }

        public async Task<bool> CreateUserAsync(RegisterDto dto)
        {
            try
            {
                if (Convert.ToBoolean(ValidateIsEmailExists(dto.Email).Result))
                {
                    throw new ArgumentException($"Email {dto.Email} is already exists");
                }

                if (Convert.ToBoolean(ValidateIsNicknameExists(dto.Nickname).Result))
                {
                    throw new ArgumentException($"Nickname {dto.Nickname} is already exists");
                }
                if (string.IsNullOrWhiteSpace(dto.Password))
                {
                    throw new ArgumentException("Password is required");
                }

                var passwordHash = GetPasswordHash(dto.Password, 
                    _configuration["ApplicationVariables:Salt"]);

                var command = new CreateUserCommand
                {
                    Id = Guid.NewGuid(),
                    Email = dto.Email,
                    NormalizedEmail = dto.Email.ToUpperInvariant(),
                    Nickname = dto.Nickname,
                    NormalizedNickname = dto.Nickname.ToUpperInvariant(),
                    RegistrationDate = DateTime.Now,
                    PasswordHash = passwordHash
                };
                await _mediator.Send(command, new CancellationToken());

                return await SetRoleAsync(command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> CheckPasswordByEmailAsync(string email, string password)
        {
            try
            {
                var user = await GetUserByEmailAsync(email);
                if (user.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(user.PasswordHash))
                    {
                        var enteredPasswordHash = GetPasswordHash(password, _configuration["ApplicationVariables:Salt"]);

                        if (user.PasswordHash.Equals(enteredPasswordHash))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(string email, string currentPass, string newPass)
        {
            try
            {
                if (!await CheckPasswordByEmailAsync(email, currentPass))
                {
                    throw new ArgumentException("Incorrect email or current password");
                }

                var user = await GetUserByEmailAsync(email);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                var password = GetPasswordHash(newPass, _configuration["ApplicationVariables:Salt"]);
                var command = new ChangePasswordCommand(user.Id, password);

                return await _mediator.Send(command, new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        private string GetPasswordHash(string password, string salt)
        {
            try
            {
                var sha1 = new SHA1CryptoServiceProvider();
                var sha1Data = sha1.ComputeHash(Encoding.UTF8.GetBytes($"{salt}_{password}"));
                var hashedPassword = Encoding.UTF8.GetString(sha1Data);
                return hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        private async Task<bool> SetRoleAsync(Guid userId)
        {
            try
            {
                var roleId = await _mediator.Send(new GetRoleIdByRoleNameQuery("USER"), 
                    new CancellationToken());

                var command = new CreateRoleCommand
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = roleId
                };
                return await _mediator.Send(command, new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        private async Task<bool> ValidateIsNicknameExists(string nickname)
        {
            try
            {
                var normNickname = nickname.ToUpperInvariant();

                return await _mediator.Send(new ValidateNicknameQuery(normNickname),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        private async Task<bool> ValidateIsEmailExists(string email)
        {
            try
            {
                var normEmail = email.ToUpperInvariant();

                return await _mediator.Send(new ValidateEmailQuery(normEmail),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
