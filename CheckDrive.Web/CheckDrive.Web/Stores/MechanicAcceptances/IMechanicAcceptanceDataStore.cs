using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public interface IMechanicAcceptanceDataStore
    {
        Task<GetMechanicResponse> GetMechanicAcceptances();
        Task<MechanicAcceptance> GetMechanicAcceptance(int id);
        Task<MechanicAcceptance> CreateMechanicAcceptance(MechanicAcceptance mechanicAcceptance);
        Task<MechanicAcceptance> UpdateMechanicAcceptance(int id, MechanicAcceptance mechanicAcceptance);
        Task DeleteMechanicAcceptance(int id);
    }
}
