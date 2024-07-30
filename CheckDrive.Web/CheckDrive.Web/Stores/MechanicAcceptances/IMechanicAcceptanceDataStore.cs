using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Constants;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public interface IMechanicAcceptanceDataStore
    {
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(int? pageNumber, string? searchString, DateTime? date, string? status, int? roleId);
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(int? pageNumber, string? searchString, DateTime? date, int? accountId);
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync();
        Task<MechanicAcceptanceDto> GetMechanicAcceptanceAsync(int id);
        Task<MechanicAcceptanceDto> CreateMechanicAcceptanceAsync(MechanicAcceptanceForCreateDto acceptanceForCreateDto);
        Task<MechanicAcceptanceDto> UpdateMechanicAcceptanceAsync(int id, MechanicAcceptanceForUpdateDto mechanicAcceptanceForUpdateDto);
        Task DeleteMechanicAcceptanceAsync(int id);
    }
}
