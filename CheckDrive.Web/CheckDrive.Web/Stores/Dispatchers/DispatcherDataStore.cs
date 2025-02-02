using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.Dispatchers;

public class DispatcherDataStore : IDispatcherDataStore
{
    private readonly CheckDriveApi _apiClient;

    public DispatcherDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<GetDispatcherResponse> GetDispatchers(int? accountId, int? roleId)
    {
        throw new NotImplementedException();
    }

    public Task<GetDispatcherResponse> GetDispatchers()
    {
        throw new NotImplementedException();
    }
}
