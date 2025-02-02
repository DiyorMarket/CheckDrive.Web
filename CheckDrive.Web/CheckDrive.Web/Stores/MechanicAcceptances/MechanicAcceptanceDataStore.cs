using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.MechanicAcceptances;

public class MechanicAcceptanceDataStore : IMechanicAcceptanceDataStore
{
    private readonly CheckDriveApi _apiClient;

    public MechanicAcceptanceDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<MechanicAcceptanceDto> CreateMechanicAcceptanceAsync(MechanicAcceptanceForCreateDto acceptanceForCreateDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMechanicAcceptanceAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetExportFile(int year, int month)
    {
        throw new NotImplementedException();
    }

    public Task<MechanicAcceptanceDto> GetMechanicAcceptanceAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(int? pageNumber, string? searchString, DateTime? date, string? status, int? roleId)
    {
        throw new NotImplementedException();
    }

    public Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(int? pageNumber, string? searchString, DateTime? date, int? accountId)
    {
        throw new NotImplementedException();
    }

    public Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<MechanicAcceptanceDto> UpdateMechanicAcceptanceAsync(int id, MechanicAcceptanceForUpdateDto mechanicAcceptanceForUpdateDto)
    {
        throw new NotImplementedException();
    }
}