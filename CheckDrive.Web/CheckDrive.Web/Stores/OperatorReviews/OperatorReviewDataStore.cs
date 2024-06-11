using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.OperatorReviews
{
    public class OperatorReviewDataStore(ApiClient api) : IOperatorReviewDataStore
    {
        private readonly ApiClient _api = api;

        public async Task<GetOperatorReviewResponse> GetOperatorReviews()
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

        public async Task<OperatorReviewDto> GetOperatorReview(int id)
        {
            var response = await _api.GetAsync($"operators/review/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch operatorReview with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<OperatorReviewDto>(json);

            return result;
        }

        public async Task<OperatorReviewDto> CreateOperatorReview(OperatorReviewForCreateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = await _api.PostAsync("operators/review", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating operatorReviews.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<OperatorReviewDto>(jsonResponse);
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
