using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.Operators
{
    public class OperatorDataStore(ApiClient apiClient) : IOperatorDataStore
    {
        private readonly ApiClient _api = apiClient;

        public async Task<GetOperatorResponse> GetOperators()
        {
            var response = await _api.GetAsync("operators?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetOperatorResponse>(json);

            return result;
        }

        public async Task<GetOperatorResponse> GetOperators(int accountId)
        {
            StringBuilder query = new("");

            if (!accountId.Equals(0))
            {
                query.Append($"accountId={accountId}");
            }

            var response = await _api.GetAsync("operators?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch operators.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetOperatorResponse>(json);

            return result;
        }

        public Task<Operator> GetOperator(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Operator> CreateOperator(Operator @operator)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOperator(int id)
        {
            throw new NotImplementedException();
        }    

        public Task<Operator> UpdateOperator(int id, Operator @operator)
        {
            throw new NotImplementedException();
        }
    }
}
