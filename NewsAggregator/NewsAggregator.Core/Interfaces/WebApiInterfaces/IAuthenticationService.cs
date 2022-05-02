using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.WebApiInterfaces
{
    public interface IAuthenticationService
    {
        Task<UserDto> CreateUserByApiAsync(RegisterDto dto);
        Task<int?> ChangePasswordByApiAsync(string email, string currentPass, string newPass);
    }
}
