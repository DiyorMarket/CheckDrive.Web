using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Drivers
{
    public interface IDriverDataStore
    {
        Task<List<Driver>> GetDrivers();
        Task<Driver> GetDriver(int id);
        Task<Driver> CreateDriver(Driver driver);
        Task<Driver> UpdateDriver(int id, Driver driver);
        Task DeleteDriver(int id);
    }
}
