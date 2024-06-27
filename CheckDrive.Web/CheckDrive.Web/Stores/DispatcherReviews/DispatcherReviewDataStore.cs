using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public class DispatcherReviewDataStore(ApiClient api) : IDispatcherReviewDataStore
    {
        private readonly ApiClient _api = api;
        public async Task<GetDispatcherReviewResponse> GetDispatcherReviews(int? pageNumber)
        {
            StringBuilder query = new("");

            if (pageNumber != null)
            {
                query.Append($"pageNumber={pageNumber}");
            }
            var response = await _api.GetAsync("dispatchers/reviews?OrderBy=datedesc&" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDispatcherReviewResponse>(json);

            return result;
        }
        public Task<DispatcherReview> GetDispatcherReview(int id)
        {
            throw new NotImplementedException();
        }
        public Task<DispatcherReview> CreateDispatcherReview(DispatcherReview review)
        {
            throw new NotImplementedException();
        }
        public Task<DispatcherReview> UpdateDispatcherReview(int id, DispatcherReview review)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDispatcherReview(int id)
        {
            throw new NotImplementedException();
        }
    }
}
