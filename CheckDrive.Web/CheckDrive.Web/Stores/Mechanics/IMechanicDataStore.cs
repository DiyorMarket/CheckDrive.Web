using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Mechanics;

public interface IMechanicDataStore
{
    Task<GetMechanicResponse> GetMechanicsAsync(string? searchString, int? pageNumber);
    Task<GetMechanicResponse> GetMechanicsAsync();
    Task<MechanicDto> GetMechanicByIdAsync(int id);
    Task<MechanicDto> CreateMechanicAsync(MechanicForCreateDto mechanicForCreate);
}
