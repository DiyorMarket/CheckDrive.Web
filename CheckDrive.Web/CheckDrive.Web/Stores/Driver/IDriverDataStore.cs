using CheckDrive.ApiContracts.Driver;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Driver;

public interface IDriverDataStore
{
    Task<GetDriverResponse> GetDriversAsync(string? searchString, int? pageNumber);
    Task<GetDriverResponse> GetDriversAsync();
    Task<DriverDto> GetDriverByIdAsync(int id);
    Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate);
}

