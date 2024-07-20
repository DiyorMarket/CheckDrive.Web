using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Driver;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Drivers
{
    public interface IDriverDataStore
    {
        Task<GetDriverResponse> GetDriversAsync(string? searchString,int? pageNumber);
        Task<GetDriverResponse> GetDriversAsync(int? roleId);
        Task<DriverDto> GetDriverAsync(int id);
        Task<IEnumerable<DriverHistoryDto>> GetDriverHistories(int Id);
        Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate);
        Task<DriverDto> UpdateDriverAsync(int id, AccountForUpdateDto driverForUpdate);
        Task DeleteDriverAsync(int id);
    }
}
