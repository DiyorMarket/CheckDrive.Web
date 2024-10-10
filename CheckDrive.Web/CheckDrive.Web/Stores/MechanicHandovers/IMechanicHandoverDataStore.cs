using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.MechanicHandovers;

public interface IMechanicHandoverDataStore
{
    Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync(string? searchString, int? pageNumber);
    Task<GetMechanicHandoverResponse> GetMechanicHandoversAsync();
    Task<MechanicHandoverDto> GetMechanicHandoverByIdAsync(int id);
    Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto handoverForCreate);
}
