using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public class DispatcherReviewDataStore(ApiClient api) : IDispatcherReviewDataStore
    {
        private readonly ApiClient _api = api;
        public async Task<GetDispatcherReviewResponse> GetDispatcherReviews(
            int? pageNumber, 
            string? searchString, 
            DateTime? date,
            int? roleId)
        {
            StringBuilder query = new("");

            if (roleId != 0)
                query.Append($"roleId={roleId}&");

            if (date is not null)
                query.Append($"date={date.Value.ToString("MM/dd/yyyy")}&");

            if (!string.IsNullOrWhiteSpace(searchString))
                query.Append($"searchString={searchString}&");

            if (pageNumber != null)
                query.Append($"pageNumber={pageNumber}");

            var response = await _api.GetAsync("dispatchers/reviews?OrderBy=datedesc&" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDispatcherReviewResponse>(json);

            return result;
        }
        public async Task<DispatcherReviewDto> GetDispatcherReview(int id)
        {
            var response = await _api.GetAsync($"dispatchers/review/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch dispatcherReviews with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<DispatcherReviewDto>(json);

            return result;
        }
        public async Task<DispatcherReviewDto> CreateDispatcherReview(DispatcherReviewForCreateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = await _api.PostAsync("dispatchers/review", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating dispatcherReviews.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DispatcherReviewDto>(jsonResponse);
        }
        public Task<DispatcherReviewForUpdateDto> UpdateDispatcherReview(int id, DispatcherReviewForUpdateDto review)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDispatcherReview(int id)
        {
            throw new NotImplementedException();
        }
    }
}
