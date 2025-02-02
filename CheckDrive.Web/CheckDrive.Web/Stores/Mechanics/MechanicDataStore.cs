using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.Mechanics;

public class MechanicDataStore : IMechanicDataStore
{
    private readonly CheckDriveApi _apiClient;

    public MechanicDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<Mechanic> CreateMechanicAsync(Mechanic mechanic)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMechanicAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Mechanic> GetMechanicAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetMechanicResponse> GetMechanics(int accountId)
    {
        throw new NotImplementedException();
    }

    public Task<GetMechanicResponse> GetMechanicsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Mechanic> UpdateMechanicAsync(int id, Mechanic mechanic)
    {
        throw new NotImplementedException();
    }
}