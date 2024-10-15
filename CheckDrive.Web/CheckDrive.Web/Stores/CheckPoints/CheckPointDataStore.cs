using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.CheckPoint
{
    public class CheckPointDataStore : ICheckPointDataStore
    {
        private readonly ApiClient _api;

        public CheckPointDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetCheckPointResponse> GetCheckPointsAsync(string? searchString, int? pageNumber)
        {
            StringBuilder query = new("");

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query.Append($"searchString={searchString}&");
            }
            if (pageNumber != null)
            {
                query.Append($"pageNumber={pageNumber}");
            }

            var response = await _api.GetAsync("checkPoints?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch checkPoints.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetCheckPointResponse>(json);

            return result;
        }

        public async Task<GetCheckPointResponse> GetCheckPointsAsync()
        {

            var response = await _api.GetAsync("checkPoints?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch checkPoints.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetCheckPointResponse>(json);

            return result;
        }

        public async Task<CheckPointDto> GetCheckPointAsync(int id)
        {
            var response = await _api.GetAsync($"checkPoints/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch checkPoint with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<CheckPointDto>(json);

            return result;
        }

        public async Task<CheckPointDto> CreateCheckPointAsync(CheckPointForCreateDto checkpointForCreate)
        {
            var json = JsonConvert.SerializeObject(checkpointForCreate);
            var response = await _api.PostAsync("cars", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating cars.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<CheckPointDto>(jsonResponse);
        }
    }
}
