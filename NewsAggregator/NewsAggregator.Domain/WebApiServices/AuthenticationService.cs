using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.WebApiInterfaces;


namespace NewsAggregator.Domain.WebApiServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<JwtService> _logger;
        private readonly IAccountService _accountService;

        public AuthenticationService(ILogger<JwtService> logger,
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }
        public async Task<UserDto> CreateUserByApiAsync(RegisterDto dto)
        {
            try
            {
                if (_accountService.ValidateIsEmailExists(dto.Email))
                {
                    throw new ArgumentException($"Email {dto.Email} is already exists");
                }

                if (_accountService.ValidateIsNicknameExists(dto.Nickname))
                {
                    throw new ArgumentException($"Nickname {dto.Nickname} is already exists");
                }

                if (string.IsNullOrWhiteSpace(dto.Password))
                {
                    throw new ArgumentException("Password is required");
                }

                var userId = await _accountService.CreateUserAsync(dto.Email, dto.Nickname);
                await _accountService.SetRoleAsync(userId, "User");
                await _accountService.SetPasswordAsync(userId, dto.Password);

                return await _accountService.GetUserByEmailAsync(dto.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> ChangePasswordByApiAsync(string email, string currentPass, string newPass)
        {
            try
            {
                if (!await _accountService.CheckPasswordByEmailAsync(email, currentPass))
                {
                    throw new ArgumentException("Incorrect email or current password");
                }

                var userId = await _accountService.GetUserIdByEmailAsync(email);
                if (userId == null)
                {
                    throw new ArgumentException("User not found");
                }
                return await _accountService.SetPasswordAsync((Guid)userId, newPass);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
