using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Drivers
{
    public interface IDriverDataStore
    {
        Task<GetDriverResponse> GetDriversAsync();
        Task<Driver> GetDriverAsync(int id);
        Task<Driver> CreateDriverAsync(Driver driver);
        Task<Driver> UpdateDriverAsync(int id, Driver driver);
        Task DeleteDriverAsync(int id);
    }
}
