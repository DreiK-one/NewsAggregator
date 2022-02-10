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
        //Task<int?> CreateAsync(UserDto userDto); 
        Task<int?> UpdateAsync(UserDto userDto);
        Task<int?> DeleteAsync(Guid id);
    }
}
