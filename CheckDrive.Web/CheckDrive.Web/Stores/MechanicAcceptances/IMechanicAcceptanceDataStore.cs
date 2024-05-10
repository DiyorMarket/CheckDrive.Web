using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public interface IMechanicAcceptanceDataStore
    {
        Task<List<MechanicAcceptance>> GetMechanicAcceptances();
        Task<MechanicAcceptance> GetMechanicAcceptance(int id);
        Task<MechanicAcceptance> CreateMechanicAcceptance(MechanicAcceptance mechanicAcceptance);
        Task<MechanicAcceptance> UpdateMechanicAcceptance(int id, MechanicAcceptance mechanicAcceptance);
        Task DeleteMechanicAcceptance(int id);
    }
}
