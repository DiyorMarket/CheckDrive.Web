using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicHandovers
{
    public interface IMechanicHandoverDataStore
    {
        Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(int? pageNumber);
        Task<MechanicHandover> GetMechanicHandoverAsync(int id);
        Task<MechanicHandover> CreateMechanicHandoverAsync(MechanicHandover mechanicHandover);
        Task<MechanicHandover> UpdateMechanicHandoverAsync(int id, MechanicHandover mechanicHandover);
        Task DeleteMechanicHandoverAsync(int id);
    }
}
