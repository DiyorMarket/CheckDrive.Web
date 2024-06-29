using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Doctor;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Doctors
{
    public interface IDoctorDataStore
    {
        Task<GetDoctorResponse> GetDoctors(int accountId);
        Task<GetDoctorResponse> GetDoctors();
        Task<DoctorDto> GetDoctor(int id);
        Task<DoctorDto> CreateDoctor(DoctorForCreateDto doctorForCreate);
        Task<DoctorDto> UpdateDoctor(int id, AccountForUpdateDto doctorForUpdate);
        Task DeleteDoctor(int id);
    }
}
