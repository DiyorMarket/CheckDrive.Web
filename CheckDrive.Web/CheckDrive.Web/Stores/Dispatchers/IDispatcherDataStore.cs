using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Dispatchers
{
    public interface IDispatcherDataStore
    {
        Task<List<Dispatcher>> GetDispatchers();
        Task<Dispatcher> GetDispatcher(int id);
        Task<Dispatcher> CreateDispatcher(Dispatcher dispatcher);
        Task<Dispatcher> UpdateDispatcher(int id, Dispatcher dispatcher);
        Task DeleteDispatcher(int id);
    }
}
