using System.Text;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.DoctorReviews
{
    public class DoctorReviewDataStore(ApiClient api) : IDoctorReviewDataStore
    {
        private readonly ApiClient _api = api;

        public async Task<GetDoctorReviewResponse> GetDoctorReviewsAsync(
            int? pageNumber,
            string? searchString,
            DateTime? date,
            bool? isHealthy,
            int? roleId,
            int? accountId)
        {
            StringBuilder query = new("");

            if(isHealthy is not null)
                query.Append($"isHealthy={isHealthy}&");

            if (roleId != 0)
                query.Append($"roleId={roleId}&");

            if (accountId != 0)
                query.Append($"accountId={accountId}&");

            if (date is not null)
                query.Append($"date={date.Value.ToString("MM/dd/yyyy")}&");

            if (!string.IsNullOrWhiteSpace(searchString))
                query.Append($"searchString={searchString}&");

            if (pageNumber != null)
                query.Append($"pageNumber={pageNumber}");

            var response = await _api.GetAsync("doctors/reviews?OrderBy=datedesc&" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch doctorReviews.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDoctorReviewResponse>(json);

            return result;
        }

        public async Task<IEnumerable<DoctorReviewDto>>? GetTodayReviewsAsync()
        {
          
            var response = await _api.GetAsync("doctors/reviews/today");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch doctorReviews.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<IEnumerable<DoctorReviewDto>>(json);

            return result;
        }

        public async Task<DoctorReviewDto> GetDoctorReviewAsync(int id)
        {
            var response = await _api.GetAsync($"doctors/review/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch doctorReview with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<DoctorReviewDto>(json);

            return result;
        }

        public async Task<DoctorReviewDto> CreateDoctorReviewAsync(DoctorReviewForCreateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = await _api.PostAsync("doctors/review", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating doctorReviews.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DoctorReviewDto>(jsonResponse);
        }

        public async Task<DoctorReviewDto> UpdateDoctorReviewAsync(int id, DoctorReviewForUpdateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = await _api.PutAsync($"doctors/review/{review.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating doctorReview.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DoctorReviewDto>(jsonResponse);
        }

        public async Task DeleteDoctorReviewAsync(int id)
        {
            var response = await _api.DeleteAsync($"doctors/review/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete Doctor reviews with id: {id}.");
            }
        }
    }
}
