using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.Roles;

public class RoleDataStore : IRoleDataStore
{
    private readonly CheckDriveApi _apiClient;

    public RoleDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<ApiContracts.Role.RoleDto> CreateRole(ApiContracts.Role.RoleForCreateDto role)
    {
        throw new NotImplementedException();
    }

    public Task CreateRole(DTOs.Role.RoleForCreateDto role)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRole(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ApiContracts.Role.RoleDto> GetRole(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetRoleResponse> GetRoles()
    {
        throw new NotImplementedException();
    }

    public Task<ApiContracts.Role.RoleDto> UpdateRole(int id, ApiContracts.Role.RoleForUpdateDto role)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRole(int id, ApiContracts.Role.RoleForCreateDto role)
    {
        throw new NotImplementedException();
    }
}
