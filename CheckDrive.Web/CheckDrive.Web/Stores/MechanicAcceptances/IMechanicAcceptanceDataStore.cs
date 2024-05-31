using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public interface IMechanicAcceptanceDataStore
    {
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync(int? pageNumber);
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync();
        Task<MechanicAcceptanceDto> GetMechanicAcceptanceAsync(int id);
        Task<MechanicAcceptanceDto> CreateMechanicAcceptanceAsync(MechanicAcceptanceForCreateDto acceptanceForCreateDto);
        Task<MechanicAcceptanceDto> UpdateMechanicAcceptanceAsync(int id, MechanicAcceptance mechanicAcceptance);
        Task DeleteMechanicAcceptanceAsync(int id);
    }
}
