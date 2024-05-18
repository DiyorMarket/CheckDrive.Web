using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Doctors
{
    public class MockDoctorDataStore : IDoctorDataStore
    {
        private readonly List<Doctor> _doctors;

        public MockDoctorDataStore()
        {
            _doctors = new List<Doctor>
            {
                new Doctor { Id = 1, AccountId = 1 },
                new Doctor { Id = 2, AccountId = 2 },
            };
        }

        public async Task<List<Doctor>> GetDoctors()
        {
            await Task.Delay(100);
            return _doctors.ToList();
        }

        public async Task<Doctor> GetDoctor(int id)
        {
            await Task.Delay(100);
            return _doctors.FirstOrDefault(d => d.Id == id);
        }

        public async Task<Doctor> CreateDoctor(Doctor doctor)
        {
            await Task.Delay(100);
            doctor.Id = _doctors.Max(d => d.Id) + 1;
            _doctors.Add(doctor);
            return doctor;
        }

        public async Task<Doctor> UpdateDoctor(int id, Doctor doctor)
        {
            await Task.Delay(100);
            var existingDoctor = _doctors.FirstOrDefault(d => d.Id == id);
            if (existingDoctor != null)
            {
                existingDoctor.AccountId = doctor.AccountId;
            }
            return existingDoctor;
        }

        public async Task DeleteDoctor(int id)
        {
            await Task.Delay(100);
            var doctorToRemove = _doctors.FirstOrDefault(d => d.Id == id);
            if (doctorToRemove != null)
            {
                _doctors.Remove(doctorToRemove);
            }
        }
    }
}
