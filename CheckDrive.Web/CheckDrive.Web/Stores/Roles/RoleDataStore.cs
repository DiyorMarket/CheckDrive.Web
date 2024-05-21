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
            var response = await _api.GetAsync("roles");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch roles.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetRoleResponse>(json);

            return result;
        }

        public async Task<Role> GetRole(int id)
        {
            var response = await _api.GetAsync($"roles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch roles with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Role>(json);

            return result;
        }

        public async Task<Role> CreateRole(Role role)
        {
            var json = JsonConvert.SerializeObject(role);
            var response = await _api.PostAsync("roles", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating roles.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Role>(jsonResponse);
        }

        public async Task<Role> UpdateRole(int id, Role role)
        {
            var json = JsonConvert.SerializeObject(role);
            var response = await _api.PutAsync($"roles/{role.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating roles.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Role>(jsonResponse);
        }

        public async Task DeleteRole(int id)
        {
            var response = await _api.DeleteAsync($"roles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete roles with id: {id}.");
            }
        }
    }
}
