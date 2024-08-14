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
            int? roleId,
            int? accountId)
        {
            StringBuilder query = new("");

            if (accountId != 0)
                query.Append($"accountId={accountId}&");

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
        public async Task<DispatcherReviewDto> UpdateDispatcherReview(int id, DispatcherReviewForUpdateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = await _api.PutAsync($"dispatchers/review/{id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating dispatcherReviews.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<DispatcherReviewDto>(jsonResponse);
        }

        public async Task DeleteDispatcherReview(int id)
        {
            var response = await _api.DeleteAsync($"dispatchers/review/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete Dispatcher reviews with id: {id}.");
            }
        }
        public async Task<Stream> GetExportFile(int year, int month)
        {
            try
            {
                string url = $"dispatchers/review/export?year={year}&month={month}";
                var response = await _api.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to retrieve the file.");
                }

                var stream = await response.Content.ReadAsStreamAsync();
                if (stream == null || stream.Length == 0)
                {
                    Console.WriteLine("The file is empty or could not be retrieved.");
                    return null; // or throw an exception based on your needs
                }

                return stream;
            }
            catch (InvalidOperationException ex)
            {
                // Log or handle the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
    }
}
