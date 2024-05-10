using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Mechanics
{
    public class MockMechanicDataStore : IMechanicDataStore
    {
        private readonly List<Mechanic> _mechanics;

        public MockMechanicDataStore()
        {
            _mechanics = new List<Mechanic>
            {
                new Mechanic { Id = 1, AccountId = 1 },
                new Mechanic { Id = 2, AccountId = 2 },
            };
        }

        public async Task<List<Mechanic>> GetMechanics()
        {
            await Task.Delay(100); 
            return _mechanics.ToList();
        }

        public async Task<Mechanic> GetMechanic(int id)
        {
            await Task.Delay(100); 
            return _mechanics.FirstOrDefault(m => m.Id == id);
        }

        public async Task<Mechanic> CreateMechanic(Mechanic mechanic)
        {
            await Task.Delay(100);
            mechanic.Id = _mechanics.Max(m => m.Id) + 1; 
            _mechanics.Add(mechanic);
            return mechanic;
        }

        public async Task<Mechanic> UpdateMechanic(int id, Mechanic mechanic)
        {
            await Task.Delay(100); 
            var existingMechanic = _mechanics.FirstOrDefault(m => m.Id == id);
            if (existingMechanic != null)
            {
                existingMechanic.AccountId = mechanic.AccountId;
            }
            return existingMechanic;
        }

        public async Task DeleteMechanic(int id)
        {
            await Task.Delay(100); 
            var mechanicToRemove = _mechanics.FirstOrDefault(m => m.Id == id);
            if (mechanicToRemove != null)
            {
                _mechanics.Remove(mechanicToRemove);
            }
        }
    }
}
