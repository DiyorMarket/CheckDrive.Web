using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicAcceptances;

public interface IMechanicAcceptanceDataStore
{
    Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(string? searchString, int? pageNumber);
    Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync();
    Task<MechanicAcceptanceDto> GetMechanicAcceptanceByIdAsync(int id);
    Task<MechanicAcceptanceDto> CreateMechanicAcceptanceAsync(MechanicAcceptanceForCreateDto acceptanceForCreate);
}

