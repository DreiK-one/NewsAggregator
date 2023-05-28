using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IRoleServiceCQS
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleAsync(Guid id);
        Task<Guid> GetRoleIdByUserIdAsync(Guid id); 
        Task<Guid> GetRoleIdByNameAsync(string name);
        Task<string> GetRoleNameByIdAsync(Guid id);
        Task<int?> CreateAsync(RoleDto roleDto);
        Task<Guid> CreateRole(string name);
        Task<int?> ChangeUserRole(UserRoleDto dto);
        Task<int?> UpdateAsync(RoleDto roleDto);
        Task<int?> DeleteAsync(Guid id); 
    }
}
