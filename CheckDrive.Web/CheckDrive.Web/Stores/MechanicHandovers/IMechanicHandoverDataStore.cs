using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicHandovers
{
    public interface IMechanicHandoverDataStore
    {
        Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(int? pageNumber, string? searchString, DateTime? date, string? status, int? roleId);
        Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(int? pageNumber, string? searchString, DateTime? date, int? accountId);
        Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync();
        Task<MechanicHandoverDto> GetMechanicHandoverAsync(int id);
        Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto mechanicHandoverForCreateDto);
        Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(int id, MechanicHandoverForUpdateDto mechanicHandoverForUpdateDto);
        Task DeleteMechanicHandoverAsync(int id);
        Task<Stream> GetExportFile(int year, int month);
    }
}
