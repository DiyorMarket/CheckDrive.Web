using CheckDrive.ApiContracts.Dispatcher;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Dispatchers;

public interface IDispatcherDataStore
{
    Task<GetDispatcherResponse> GetDispatchersAsync();
    Task<DispatcherDto> GetDispatcherByIdAsync(int id);
    Task<DispatcherDto> CreateDispatcherAsync(DispatcherForCreateDto dispatcherForCreate);
}
