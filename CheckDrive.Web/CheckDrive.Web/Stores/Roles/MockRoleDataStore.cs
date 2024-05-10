using CheckDrive.Web.Models;
using System.Collections.Immutable;

namespace CheckDrive.Web.Stores.Roles
{
    public class MockRoleDataStore : IRoleDataStore
    {
        private readonly List<Role> _roles;

        public MockRoleDataStore()
        {
            _roles = new List<Role>
            {
               new Role{Id = 1, Name = "Manager"},
               new Role{Id = 2, Name = "Driver"}
            };
        }
        
        public async Task<List<Role>> GetRoles()
        {
            await Task.Delay(100);
            return _roles.ToList();
        }

        public async Task<Role> GetRole(int id)
        {
            await Task.Delay(100);
            return _roles.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Role> CreateRole(Role role)
        {
            await Task.Delay(100);
            role.Id = _roles.Max(r => r.Id) + 1;
            _roles.Add(role);
            return role;
        }

        public async Task<Role> UpdateRole(int id, Role role)
        {
            await Task.Delay(100);
            var exectingRole = _roles.FirstOrDefault(role => role.Id == id);
            if (exectingRole != null) 
            {
               exectingRole.Name = role.Name;
            }
            return exectingRole;

        }

        public async Task DeleteRole(int id)
        {
            await Task.Delay(100);
            var exectingRole = _roles.FirstOrDefault(role => role.Id == id);
            if (exectingRole != null)
            {
                 _roles.Remove(exectingRole);
            }

        }
    }
}
