using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.MechanicHandovers
{
    public interface IMechanicHandoverDataStore
    {
        Task<List<MechanicHandover>> GetMechanicHandovers();
        Task<MechanicHandover> GetMechanicHandover(int id);
        Task<MechanicHandover> CreateMechanicHandover(MechanicHandover mechanicHandover);
        Task<MechanicHandover> UpdateMechanicHandover(int id, MechanicHandover mechanicHandover);
        Task DeleteMechanicHandover(int id);
    }
}
