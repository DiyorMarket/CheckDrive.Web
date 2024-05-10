using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.MechanicAcceptances
{
    public class MockMechanicAcceptanceDataStore : IMechanicAcceptanceDataStore
    {
        private readonly List<MechanicAcceptance> _mechanicAcceptances;

        public MockMechanicAcceptanceDataStore()
        {
            _mechanicAcceptances = new List<MechanicAcceptance>
            {
                new MechanicAcceptance { Id = 1, IsAccepted = true, Comments = "Completed", Status = Status.Completed, Date = DateTime.Now, Distance = 10, MechanicHandoverId = 1 },
                new MechanicAcceptance { Id = 2, IsAccepted = false, Comments = "Rejected", Status = Status.Rejected, Date = DateTime.Now, Distance = 20, MechanicHandoverId = 2 },
            };
        }

        public async Task<List<MechanicAcceptance>> GetMechanicAcceptances()
        {
            await Task.Delay(100); 
            return _mechanicAcceptances.ToList();
        }

        public async Task<MechanicAcceptance> GetMechanicAcceptance(int id)
        {
            await Task.Delay(100);
            return _mechanicAcceptances.FirstOrDefault(ma => ma.Id == id);
        }

        public async Task<MechanicAcceptance> CreateMechanicAcceptance(MechanicAcceptance mechanicAcceptance)
        {
            await Task.Delay(100); 
            mechanicAcceptance.Id = _mechanicAcceptances.Max(ma => ma.Id) + 1;
            _mechanicAcceptances.Add(mechanicAcceptance);
            return mechanicAcceptance;
        }

        public async Task<MechanicAcceptance> UpdateMechanicAcceptance(int id, MechanicAcceptance mechanicAcceptance)
        {
            await Task.Delay(100); 
            var existingMechanicAcceptance = _mechanicAcceptances.FirstOrDefault(ma => ma.Id == id);
            if (existingMechanicAcceptance != null)
            {
                existingMechanicAcceptance.IsAccepted = mechanicAcceptance.IsAccepted;
                existingMechanicAcceptance.Comments = mechanicAcceptance.Comments;
                existingMechanicAcceptance.Status = mechanicAcceptance.Status;
                existingMechanicAcceptance.Date = mechanicAcceptance.Date;
                existingMechanicAcceptance.Distance = mechanicAcceptance.Distance;
                existingMechanicAcceptance.MechanicHandoverId = mechanicAcceptance.MechanicHandoverId;
            }
            return existingMechanicAcceptance;
        }

        public async Task DeleteMechanicAcceptance(int id)
        {
            await Task.Delay(100); 
            var mechanicAcceptanceToRemove = _mechanicAcceptances.FirstOrDefault(ma => ma.Id == id);
            if (mechanicAcceptanceToRemove != null)
            {
                _mechanicAcceptances.Remove(mechanicAcceptanceToRemove);
            }
        }
    }
}
