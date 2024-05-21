using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public interface IMechanicAcceptanceDataStore
    {
        Task<GetMechanicAcceptanceResponse> GetMechanicAcceptancesAsync();
        Task<MechanicAcceptance> GetMechanicAcceptanceAsync(int id);
        Task<MechanicAcceptance> CreateMechanicAcceptanceAsync(MechanicAcceptance mechanicAcceptance);
        Task<MechanicAcceptance> UpdateMechanicAcceptanceAsync(int id, MechanicAcceptance mechanicAcceptance);
        Task DeleteMechanicAcceptanceAsync(int id);
    }
}
