using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Driver;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.Drivers;

public class DriverDataStore : IDriverDataStore
{
    private readonly CheckDriveApi _apiClient;

    public DriverDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDriverAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DriverDto> GetDriverAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DriverHistoryDto>> GetDriverHistories(int Id)
    {
        throw new NotImplementedException();
    }

    public Task<GetDriverResponse> GetDriversAsync(string? searchString, int? pageNumber)
    {
        throw new NotImplementedException();
    }

    public Task<GetDriverResponse> GetDriversAsync(int? roleId, bool? isBusy)
    {
        throw new NotImplementedException();
    }

    public Task<DriverDto> UpdateDriverAsync(int id, AccountForUpdateDto driverForUpdate)
    {
        throw new NotImplementedException();
    }
}
