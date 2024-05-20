using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.Operators
{
    public class OperatorDataStore(ApiClient apiClient) : IOperatorDataStore
    {
        private readonly ApiClient _api = apiClient;

        public async Task<GetOperatorResponse> GetOperators()
        {
            var response = _api.Get("operators?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetOperatorResponse>(json);

            return result;
        }

        public async Task<GetOperatorReviewResponse> GetOperatorsReviews()
        {
            var response = _api.Get("operators/reviews");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetOperatorReviewResponse>(json);

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
