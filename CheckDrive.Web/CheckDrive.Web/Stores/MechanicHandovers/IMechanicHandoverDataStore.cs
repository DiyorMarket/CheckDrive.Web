using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicHandovers
{
    public interface IMechanicHandoverDataStore
    {
        Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync();
        Task<MechanicHandover> GetMechanicHandoverAsync(int id);
        Task<MechanicHandover> CreateMechanicHandoverAsync(MechanicHandover mechanicHandover);
        Task<MechanicHandover> UpdateMechanicHandoverasync(int id, MechanicHandover mechanicHandover);
        Task DeleteMechanicHandoverAsync(int id);
    }
}
