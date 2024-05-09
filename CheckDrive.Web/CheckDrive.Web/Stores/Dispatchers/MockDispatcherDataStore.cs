using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Dispatchers
{
    public class MockDispatcherDataStore : IDispatcherDataStore
    {
        private readonly List<Dispatcher> _dispatchers;

        public MockDispatcherDataStore()
        {
            _dispatchers = new List<Dispatcher>
            {
                new Dispatcher { Id = 1, AccountId = 1 },
                new Dispatcher { Id = 2, AccountId = 2 },
            };
        }

        public async Task<List<Dispatcher>> GetDispatchers()
        {
            await Task.Delay(100); 
            return _dispatchers.ToList();
        }

        public async Task<Dispatcher> GetDispatcher(int id)
        {
            await Task.Delay(100); 
            return _dispatchers.FirstOrDefault(d => d.Id == id);
        }

        public async Task<Dispatcher> CreateDispatcher(Dispatcher dispatcher)
        {
            await Task.Delay(100); 
            dispatcher.Id = _dispatchers.Max(d => d.Id) + 1; 
            _dispatchers.Add(dispatcher);
            return dispatcher;
        }

        public async Task<Dispatcher> UpdateDispatcher(int id, Dispatcher dispatcher)
        {
            await Task.Delay(100); 
            var existingDispatcher = _dispatchers.FirstOrDefault(d => d.Id == id);
            if (existingDispatcher != null)
            {
                existingDispatcher.AccountId = dispatcher.AccountId;
            }
            return existingDispatcher;
        }

        public async Task DeleteDispatcher(int id)
        {
            await Task.Delay(100); 
            var dispatcherToRemove = _dispatchers.FirstOrDefault(d => d.Id == id);
            if (dispatcherToRemove != null)
            {
                _dispatchers.Remove(dispatcherToRemove);
            }
        }
    }
}
