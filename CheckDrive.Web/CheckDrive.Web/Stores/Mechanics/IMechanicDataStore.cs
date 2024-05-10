using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Mechanics
{
    public interface IMechanicDataStore
    {
        Task<List<Mechanic>> GetMechanics();
        Task<Mechanic> GetMechanic(int id);
        Task<Mechanic> CreateMechanic(Mechanic mechanic);
        Task<Mechanic> UpdateMechanic(int id, Mechanic mechanic);
        Task DeleteMechanic(int id);
    }
}
