using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.MechanicHandovers
{
    public class MockMechanicHandoverDataStore : IMechanicHandoverDataStore
    {
        private readonly List<MechanicHandover> _mechanicHandovers;

        public MockMechanicHandoverDataStore()
        {
            _mechanicHandovers = new List<MechanicHandover>
            {
                new MechanicHandover { Id = 1, IsHanded = true, Comments = "Completed", Status = Status.Completed, Date = DateTime.Now, MechanicId = 1, CarId = 1, DriverId = 1 },
                new MechanicHandover { Id = 2, IsHanded = false, Comments = "Rejected", Status = Status.Rejected, Date = DateTime.Now, MechanicId = 2, CarId = 2, DriverId = 2 },
            };
        }

        public async Task<List<MechanicHandover>> GetMechanicHandovers()
        {
            await Task.Delay(100);
            return _mechanicHandovers.ToList();
        }

        public async Task<MechanicHandover> GetMechanicHandover(int id)
        {
            await Task.Delay(100); 
            return _mechanicHandovers.FirstOrDefault(mh => mh.Id == id);
        }

        public async Task<MechanicHandover> CreateMechanicHandover(MechanicHandover mechanicHandover)
        {
            await Task.Delay(100); 
            mechanicHandover.Id = _mechanicHandovers.Max(mh => mh.Id) + 1;
            _mechanicHandovers.Add(mechanicHandover);
            return mechanicHandover;
        }

        public async Task<MechanicHandover> UpdateMechanicHandover(int id, MechanicHandover mechanicHandover)
        {
            await Task.Delay(100); 
            var existingMechanicHandover = _mechanicHandovers.FirstOrDefault(mh => mh.Id == id);
            if (existingMechanicHandover != null)
            {
                existingMechanicHandover.IsHanded = mechanicHandover.IsHanded;
                existingMechanicHandover.Comments = mechanicHandover.Comments;
                existingMechanicHandover.Status = mechanicHandover.Status;
                existingMechanicHandover.Date = mechanicHandover.Date;
                existingMechanicHandover.MechanicId = mechanicHandover.MechanicId;
                existingMechanicHandover.CarId = mechanicHandover.CarId;
                existingMechanicHandover.DriverId = mechanicHandover.DriverId;
            }
            return existingMechanicHandover;
        }

        public async Task DeleteMechanicHandover(int id)
        {
            await Task.Delay(100);
            var mechanicHandoverToRemove = _mechanicHandovers.FirstOrDefault(mh => mh.Id == id);
            if (mechanicHandoverToRemove != null)
            {
                _mechanicHandovers.Remove(mechanicHandoverToRemove);
            }
        }
    }
}
