using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicHandovers
{
    public interface IMechanicHandoverDataStore
    {
        Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(int? pageNumber);
        Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync();
        Task<MechanicHandoverDto> GetMechanicHandoverAsync(int id);
        Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto mechanicHandoverForCreateDto);
        Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(int id, MechanicHandover mechanicHandover);
        Task DeleteMechanicHandoverAsync(int id);
    }
}
