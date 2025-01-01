using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.MechanicHandovers;

public class MechanicHandoverDataStore : IMechanicHandoverDataStore
{
    private readonly CheckDriveApi _apiClient;

    public MechanicHandoverDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto mechanicHandoverForCreateDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMechanicHandoverAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetExportFile(int year, int month)
    {
        throw new NotImplementedException();
    }

    public Task<MechanicHandoverDto> GetMechanicHandoverAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(int? pageNumber, string? searchString, DateTime? date, string? status, int? roleId)
    {
        throw new NotImplementedException();
    }

    public Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(int? pageNumber, string? searchString, DateTime? date, int? accountId)
    {
        throw new NotImplementedException();
    }

    public Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync()
    {
        throw new NotImplementedException();
    }

    public Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(int id, MechanicHandoverForUpdateDto mechanicHandoverForUpdateDto)
    {
        throw new NotImplementedException();
    }
}