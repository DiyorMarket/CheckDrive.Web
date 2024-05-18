using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Roles
{
    public interface IRoleDataStore
    {
        Task<GetRoleResponse> GetRoles();
        Task<Role> GetRole(int id);
        Task<Role> CreateRole(Role role);
        Task<Role> UpdateRole(int id, Role role);
        Task DeleteRole(int id);
    }
}
