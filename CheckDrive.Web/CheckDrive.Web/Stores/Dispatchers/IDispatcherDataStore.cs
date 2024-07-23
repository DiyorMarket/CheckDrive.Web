using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Dispatchers
{
    public interface IDispatcherDataStore
    {
        Task<GetDispatcherResponse> GetDispatchers(int? accountId, int? roleId);
        Task<GetDispatcherResponse> GetDispatchers();
    }
}
