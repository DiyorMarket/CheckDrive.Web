using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.OperatorReviews
{
    public class OperatorReviewDataStore(ApiClient api) : IOperatorReviewDataStore
    {
        private readonly ApiClient _api = api;

        public async Task<GetOperatorReviewResponse> GetOperatorsReviews()
        {
            var response = await _api.GetAsync("operators/reviews");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetOperatorReviewResponse>(json);

            return result;
        }

        public Task<OperatorReview> GetOperatorReview(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperatorReview> CreateOperatorReview(OperatorReview operatorReview)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOperatorReview(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperatorReview> UpdateOperatorReview(int id, OperatorReview operatorReview)
        {
            throw new NotImplementedException();
        }
    }
}
