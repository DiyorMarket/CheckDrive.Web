using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Mechanics
{
    public interface IMechanicDataStore
    {
        Task<GetMechanicResponse> GetMechanicsAsync();
        Task<Mechanic> GetMechanicAsync(int id);
        Task<Mechanic> CreateMechanicAsync(Mechanic mechanic);
        Task<Mechanic> UpdateMechanicAsync(int id, Mechanic mechanic);
        Task DeleteMechanicAsync(int id);
    }
}
