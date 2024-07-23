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

        public async Task<GetDispatcherResponse> GetDispatchers(int? accountIdt, int? roleId)
        {
            StringBuilder query = new("");

            if (accountIdt != null)
            {
                query.Append($"accountId={accountIdt}");
            }

            if (roleId != null)
            {
                query.Append($"roleId={roleId}");
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
