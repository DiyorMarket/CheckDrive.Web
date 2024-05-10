using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Doctors
{
    public interface IDoctorDataStore
    {
        Task<List<Doctor>> GetDoctors();
        Task<Doctor> GetDoctor(int id);
        Task<Doctor> CreateDoctor(Doctor doctor);
        Task<Doctor> UpdateDoctor(int id, Doctor doctor);
        Task DeleteDoctor(int id);
    }
}
