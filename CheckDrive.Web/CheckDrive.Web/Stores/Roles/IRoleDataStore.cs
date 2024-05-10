using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Roles
{
    public interface IRoleDataStore
    {
        Task<List<Role>> GetRoles();
        Task<Role> GetRole(int id);
        Task<Role> CreateRole(Role role);
        Task<Role> UpdateRole(int id, Role role);
        Task DeleteRole(int id);
    }
}
