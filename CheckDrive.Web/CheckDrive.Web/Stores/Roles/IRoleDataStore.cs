using CheckDrive.ApiContracts.Role;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Roles
{
    public interface IRoleDataStore
    {
        Task<GetRoleResponse> GetRoles();
        Task<RoleDto> GetRole(int id);
        Task<RoleDto> CreateRole(RoleForCreateDto role);
        Task<RoleDto> UpdateRole(int id, RoleForUpdateDto role);
        Task DeleteRole(int id);
        Task CreateRole(DTOs.Role.RoleForCreateDto role);
        Task UpdateRole(int id, RoleForCreateDto role);
    }
}
