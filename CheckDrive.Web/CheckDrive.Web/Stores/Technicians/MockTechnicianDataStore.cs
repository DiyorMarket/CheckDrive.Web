using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Technicians
{
    public class MockTechnicianDataStore : ITechnicianDataStore
    {
        private readonly List<Technician> _technician;

        public MockTechnicianDataStore()
        {
            _technician = new List<Technician>
            {
                new Technician{ Id = 1, AccountId = 1 },
                new Technician{ Id = 2, AccountId = 2 },
            };
        }

        public async Task<List<Technician>> GetTechnicians()
        {
            await Task.Delay(100);
            return _technician.ToList();
        }

        public async Task<Technician> GetTechnician(int id)
        {
            await Task.Delay(100);
            return _technician.FirstOrDefault(t => t.Id == id);
        }

        public async Task<Technician> CreateTechnician(Technician technician)
        {
            await Task.Delay(100);
            technician.Id = _technician.Max(t => t.Id) + 1;
            _technician.Add(technician);
            return technician;

        }

        public async Task<Technician> UpdateTechnician(int id, Technician technician)
        {
            await Task.Delay(100);
            var exectingTechnician = _technician.FirstOrDefault(t => t.Id == id);
            if (exectingTechnician != null)
            {
                exectingTechnician.AccountId = technician.AccountId;
            }
            return exectingTechnician;
        }

        public async Task DeleteTechnician(int id)
        {
            await Task.Delay(100);
            var exectingTechnician = _technician.FirstOrDefault(t => t.Id == id);
            if (exectingTechnician != null)
            {
                _technician.Remove(exectingTechnician);
            }
        }
    }
}
