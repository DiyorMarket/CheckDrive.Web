using CheckDrive.ApiContracts.Doctor;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Doctors
{
    public interface IDoctorDateStore
    {
        Task<GetDoctorResponse> GetDoctorsAsync(string? searchString, int? pageNumber);
        Task<GetDoctorResponse> GetDoctorsAsync();
        Task<DoctorDto> GetDoctorByIdAsync(int id);
        Task<DoctorDto> CreateDoctorAsync(DoctorForCreateDto doctorForCreate);
    }
}
