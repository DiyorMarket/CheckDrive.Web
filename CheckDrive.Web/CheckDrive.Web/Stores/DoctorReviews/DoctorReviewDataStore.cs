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

        public async Task<GetDoctorReviewResponse> GetDoctorReviews()
        {
            var response = _api.Get("doctors/reviews");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch doctorReviews.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDoctorReviewResponse>(json);

            return result;
        }

        public async Task<DoctorReviewDto> GetDoctorReview(int id)
        {
            var response = _api.Get($"doctors/review/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch doctorReview with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<DoctorReviewDto>(json);

            return result;
        }

        public async Task<DoctorReviewDto> CreateDoctorReview(DoctorReviewForCreateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = _api.Post("doctors/review", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating doctorReviews.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DoctorReviewDto>(jsonResponse);
        }

        public async Task<DoctorReview> UpdateDoctorReview(int id, DoctorReviewForUpdateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = _api.Put($"doctors/review/{review.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating doctorReview.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DoctorReview>(jsonResponse);
        }

        public async Task DeleteDoctorReview(int id)
        {
            throw new NotImplementedException();
        }
    }
}
