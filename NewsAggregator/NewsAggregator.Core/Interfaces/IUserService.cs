using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
