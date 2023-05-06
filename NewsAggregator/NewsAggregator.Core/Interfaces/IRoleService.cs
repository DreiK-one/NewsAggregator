using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<Guid> GetRoleIdByUserIdAsync(Guid id);
        Task<int?> CreateAsync(RoleDto roleDto);
        Task<int?> UpdateAsync(RoleDto roleDto);
        Task<int?> DeleteAsync(Guid id);
        Task<RoleDto> GetRoleAsync(Guid id);
        Task<Guid> GetRoleIdByNameAsync(string name);
        Task<Guid> CreateRole(string name);
        Task<string> GetRoleNameByIdAsync(Guid id);
        Task<int?> ChangeUserRole(UserRoleDto dto);
    }
}
