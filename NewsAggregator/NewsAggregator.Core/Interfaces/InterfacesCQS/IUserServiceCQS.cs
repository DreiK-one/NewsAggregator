using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.InterfacesCQS
{
    public interface IUserServiceCQS
    {
        Task<IEnumerable<UserDto>> GetAllUsersWithAllInfoAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<int?> CreateAsync(CreateOrEditUserDto userDto);
        Task<int?> UpdateAsync(CreateOrEditUserDto userDto);
        Task<int?> DeleteAsync(Guid id);  
    }
}
