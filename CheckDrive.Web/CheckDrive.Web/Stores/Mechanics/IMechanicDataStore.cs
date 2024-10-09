using CheckDrive.DTOs.Mechanic;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Mechanics
{
    public interface IMechanicDataStore
    {
        Task<GetMechanicResponse> GetMechanicsAsync(string? searchString, int? pageNumber);
        Task<GetMechanicResponse> GetDoctorsAsync();
        Task<MechanicDto> GetDoctorByIdAsync(int id);
        Task<MechanicDto> CreateDoctorAsync(MechanicForCreateDto mechanicForCreate);
    }
}
