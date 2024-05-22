using CheckDrive.ApiContracts.Role;
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

        public async Task<RoleDto> GetRole(int id)
        {
            var response = _api.Get($"roles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch roles with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<RoleDto>(json);

            return result;
        }

        public async Task<Role> CreateRole(RoleForCreateDto role)
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

        public async Task<RoleDto> UpdateRole(int id, RoleForUpdateDto role)
        {
            var json = JsonConvert.SerializeObject(role);
            var response = _api.Put($"roles/{role.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating roles.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<RoleDto>(jsonResponse);

        }

        public async Task DeleteRole(int id)
        {
            var response = _api.Delete($"roles/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete roles with id: {id}.");
            }
        }

        Task<RoleDto> IRoleDataStore.CreateRole(RoleForCreateDto role)
        {
            throw new NotImplementedException();
        }

        public Task CreateRole(DTOs.Role.RoleForCreateDto role)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRole(int id, RoleForCreateDto role)
        {
            throw new NotImplementedException();
        }
    }
}
