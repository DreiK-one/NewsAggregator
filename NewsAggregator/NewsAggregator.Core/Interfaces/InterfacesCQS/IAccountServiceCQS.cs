using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IAccountServiceCQS
    {
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> GetUserByRefreshTokenAsync(string token);
        Task<Guid?> GetUserIdByEmailAsync(string email);
        Task<Guid> GetUserIdByNicknameAsync(string nickname);
        Task<string?> GetUserNicknameByIdAsync(Guid id);
        Task<IEnumerable<string>> GetRolesAsync(Guid userId);
        Task<bool> CreateUserAsync(RegisterDto dto);
        Task<int> SetPasswordAsync(Guid userId, string password);
        Task<bool> CheckPasswordByEmailAsync(string email, string password);
        Task<bool> CheckPasswordByIdAsync(Guid id, string password);
        Task<bool> ChangePasswordAsync(string email, string currentPass, string newPass);
        Task<int> UpdateEmail(Guid userId, string email);
        Task<int> UpdateNickname(Guid userId, string nickname);
        Task<bool> ValidateIsNicknameExists(string nickname);
        Task<bool> ValidateIsEmailExists(string email);
    }
}
