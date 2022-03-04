using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<int?> CreateAsync(RoleDto roleDto);
        Task<int?> UpdateAsync(RoleDto roleDto);
        Task<int?> DeleteAsync(Guid id);
        Task<RoleDto> GetRoleAsync(Guid Id);
    }
}
