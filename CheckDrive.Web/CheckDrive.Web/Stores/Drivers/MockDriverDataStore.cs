using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Drivers
{
    public class MockDriverDataStore : IDriverDataStore
    {
        private readonly List<Driver> _drivers;

        public MockDriverDataStore()
        {
            _drivers = new List<Driver>
            {
                new Driver { Id = 1, AccountId = 1 },
                new Driver { Id = 2, AccountId = 2 },
            };
        }

        public async Task<List<Driver>> GetDrivers()
        {
            await Task.Delay(100); 
            return _drivers.ToList();
        }

        public async Task<Driver> GetDriver(int id)
        {
            await Task.Delay(100); 
            return _drivers.FirstOrDefault(d => d.Id == id);
        }

        public async Task<Driver> CreateDriver(Driver driver)
        {
            await Task.Delay(100); 
            driver.Id = _drivers.Max(d => d.Id) + 1; 
            _drivers.Add(driver);
            return driver;
        }

        public async Task<Driver> UpdateDriver(int id, Driver driver)
        {
            await Task.Delay(100); 
            var existingDriver = _drivers.FirstOrDefault(d => d.Id == id);
            if (existingDriver != null)
            {
                existingDriver.AccountId = driver.AccountId;
               
            }
            return existingDriver;
        }

        public async Task DeleteDriver(int id)
        {
            await Task.Delay(100); 
            var driverToRemove = _drivers.FirstOrDefault(d => d.Id == id);
            if (driverToRemove != null)
            {
                _drivers.Remove(driverToRemove);
            }
        }
    }
}
