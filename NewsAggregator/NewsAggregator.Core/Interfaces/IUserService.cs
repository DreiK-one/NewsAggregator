using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersWithAllInfoAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<int?> UpdateAsync(CreateOrEditUserDto userDto);
        Task<int?> DeleteAsync(Guid id);
        Task<int?> CreateAsync(CreateOrEditUserDto userDto);
    }
}
