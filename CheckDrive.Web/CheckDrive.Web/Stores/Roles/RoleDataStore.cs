using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.Roles
{
    public class RoleDataStore : IRoleDataStore
    {
        private readonly ApiClient _api;

        public RoleDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }
        
        public async Task<GetRoleResponse> GetRoles()
        {
            var response = _api.Get("roles?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch roles.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetRoleResponse>(json);

            return result;
        }

        public async Task<Role> GetRole(int id)
        {
            var response = _api.Get($"roles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch roles with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Role>(json);

            return result;
        }

        public async Task<Role> CreateRole(Role role)
        {
            var json = JsonConvert.SerializeObject(role);
            var response = _api.Post("roles", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating roles.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<Role>(jsonResponse);
        }

        public async Task<Role> UpdateRole(int id, Role role)
        {
            var json = JsonConvert.SerializeObject(role);
            var response = _api.Put($"roles/{role.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating roles.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<Role>(jsonResponse);

        }

        public async Task DeleteRole(int id)
        {
            var response = _api.Delete($"roles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete roles with id: {id}.");
            }
        }
    }
}
