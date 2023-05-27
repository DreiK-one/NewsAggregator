using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Queries.AccountQueries;
using System.Text;
using System.Security.Cryptography;
using NewsAggregetor.CQS.Models.Commands.AccountCommands;
using NewsAggregator.Core.Helpers;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class AccountServiceCQS : IAccountServiceCQS
    {
        private readonly ILogger<AccountServiceCQS> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public AccountServiceCQS(ILogger<AccountServiceCQS> logger,
            IConfiguration configuration,
            IMediator mediator)
        {
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<UserDto> GetUserByRefreshTokenAsync(string token)
        {
            try
            {
                return await _mediator.Send(new GetUserByRefreshToken(token),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid?> GetUserIdByEmailAsync(string email)
        {
            try
            {
                return await _mediator.Send(new GetUserIdByEmailAsyncQuery(email), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid> GetUserIdByNicknameAsync(string nickname)
        {
            try
            {
                return await _mediator.Send(new GetUserIdByNicknameAsyncQuery(nickname),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<string?> GetUserNicknameByIdAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetUserNicknameByIdAsyncQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        //todo
        public Task<IEnumerable<string>> GetRolesAsync(Guid userId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
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
                    _configuration[Variables.ConfigurationFields.Salt]);

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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int> SetPasswordAsync(Guid userId, string password)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
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
                        var enteredPasswordHash = GetPasswordHash(password, _configuration[Variables.ConfigurationFields.Salt]);

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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<bool> CheckPasswordByIdAsync(Guid id, string password)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
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

                var password = GetPasswordHash(newPass, _configuration[Variables.ConfigurationFields.Salt]);
                var command = new ChangePasswordCommand(user.Id, password);

                return await _mediator.Send(command, new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int> UpdateEmail(Guid userId, string email)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int> UpdateNickname(Guid userId, string nickname)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<bool> ValidateIsNicknameExists(string nickname)
        {
            try
            {
                var normNickname = nickname.ToUpperInvariant();

                return await _mediator.Send(new ValidateNicknameQuery(normNickname),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<bool> ValidateIsEmailExists(string email)
        {
            try
            {
                var normEmail = email.ToUpperInvariant();

                return await _mediator.Send(new ValidateEmailQuery(normEmail),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        private async Task<bool> SetRoleAsync(Guid userId)
        {
            try
            {
                var roleId = await _mediator.Send(new GetRoleIdByRoleNameQuery(Variables.Roles.User),
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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }  
    }
}
