using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces
{
    public interface IAccountService
    {
        Task<bool> CheckUserWithThatEmailIsExistAsync(string email);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserById(Guid id);
        Task<UserDto> GetUserByRefreshTokenAsync(string refreshToken);
        Task<Guid> CreateUserAsync(string email, string nickname); 
        Task<int> SetRoleAsync(Guid userId, string roleName);
        Task<IEnumerable<string>> GetRolesAsync(Guid userId);
        Task<int> SetPasswordAsync(Guid userId, string password);
        Task<bool> CheckPasswordByEmailAsync(string email, string password);
        Task<bool> CheckPasswordByIdAsync(Guid id, string password);
        Task<Guid?> GetUserIdByEmailAsync(string email);
        Task<Guid> GetUserIdByNicknameAsync(string nickname);
        Task<string?> GetUserNicknameByIdAsync(Guid id);
        Task<int> UpdateEmail(Guid userId, string email);
        Task<int> UpdateNickname(Guid userId, string nickname);


        bool ValidateIsEmailExists(string email);
        bool ValidateIsNicknameExists(string nickname);
    }
}
