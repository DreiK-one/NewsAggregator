using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IAccountServiceCQS
    {
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> GetUserByRefreshTokenAsync(string token);
        Task<bool> CreateUserAsync(RegisterDto dto);
        
        Task<bool> CheckPasswordByEmailAsync(string email, string password);
        Task<bool> ChangePasswordAsync(string email, string currentPass, string newPass);

        Task<bool> ValidateIsNicknameExists(string nickname);
        Task<bool> ValidateIsEmailExists(string email);
    }
}
