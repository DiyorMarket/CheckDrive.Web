using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.OilMarks
{
    public class OilMarkDataStore : IOilMarkDataStore
    {
        private readonly ApiClient _api;

        public OilMarkDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetOilMarkResponse> GetOilMarksAsync(string? searchString, int? pageNumber)
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

            var response = await _api.GetAsync("oilmarks?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch oilmarks.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetOilMarkResponse>(json);

            return result;
        }

        public async Task<GetOilMarkResponse> GetOilMarksAsync()
        {

            var response = await _api.GetAsync("oilmarks?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch oilmarks.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetOilMarkResponse>(json);

            return result;
        }

        public async Task<OilMarkDto> GetOilMarkAsync(int id)
        {
            var response = await _api.GetAsync($"oilmarks/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch oilmark with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<OilMarkDto>(json);

            return result;
        }

        public async Task<OilMarkDto> CreateOilMarkAsync(OilMarkForCreateDto oilmarkForCreate)
        {
            var json = JsonConvert.SerializeObject(oilmarkForCreate);
            var response = await _api.PostAsync("oilmarks", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating oilmark.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<OilMarkDto>(jsonResponse);
        }
    }
}
