using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.OperatorReviews
{
    public class OperatorReviewDataStore(ApiClient api) : IOperatorReviewDataStore
    {
        private readonly ApiClient _api = api;

        public async Task<GetOperatorReviewResponse> GetOperatorReviews(
            int? pageNumber,
            string? searchString,
            DateTime? date,
            string? status,
            int? roleId,
            int? accountId)
        {
            StringBuilder query = new("");

            if (!string.IsNullOrWhiteSpace(status))
                query.Append($"status={status}&");

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

            var response = await _api.GetAsync("operators/reviews?OrderBy=datedesc&" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch operatorReviews.");
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

        public async Task<OperatorReviewDto> UpdateOperatorReview(int id, OperatorReviewForUpdateDto operatorReview)
        {
            var json = JsonConvert.SerializeObject(operatorReview);
            var response = await _api.PutAsync($"operators/review/{operatorReview.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating operatorReviews.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<OperatorReviewDto>(jsonResponse);
        }
        public async Task DeleteOperatorReview(int id)
        {

            var response = await _api.DeleteAsync($"operators/review/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete Operator reviews with id: {id}.");
            }
        }
        public async Task<Stream> GetExportFile(int year, int month)
        {
            try
            {
                string url = $"operator/review/export?year={year}&month={month}";
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
