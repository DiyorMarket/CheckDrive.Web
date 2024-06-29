using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.Dispatchers
{
    public class DispatcherDataStore : IDispatcherDataStore
    {
        private readonly ApiClient _api;

        public DispatcherDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetDispatcherResponse> GetDispatchers(int accountId)
        {
            StringBuilder query = new("");

            if (!accountId.Equals(0))
            {
                query.Append($"accountId={accountId}");
            }

            var response = await _api.GetAsync("dispatchers?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch dispatchers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDispatcherResponse>(json);

            return result;
        }

        public async Task<GetDispatcherResponse> GetDispatchers()
        {
            var response = await _api.GetAsync("dispatchers?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch dispatchers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDispatcherResponse>(json);

            return result;
        }
    }
}
