using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.OilMarks
{
    public class OilMarkDataStore(ApiClient api) : IOilMarkDataStore
    {
        private readonly ApiClient _api = api;

        public async Task<GetOilMarkResponse> GetOilMarksAsync()
        {
            var response = await _api.GetAsync("oilMarks?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch oilMarks.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetOilMarkResponse>(json);

            return result;
        }
        public async Task<OilMarkDto> GetOilMarkByIdAsync(int id)
        {
            var response = await _api.GetAsync($"oilMarks/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch oilMark with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<OilMarkDto>(json);

            return result;
        }
        public async Task<OilMarkDto> CreateOilMarkAsync(OilMarkForCreateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = await _api.PostAsync("oilMarks", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating oilMarks.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<OilMarkDto>(jsonResponse);
        }

        public async Task<OilMarkDto> UpdateOilMarkAsync(int id, OilMarkForUpdateDto review)
        {
            var json = JsonConvert.SerializeObject(review);
            var response = await _api.PutAsync($"oilMarks/{review.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating oilMark.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<OilMarkDto>(jsonResponse);
        }
        public async Task DeleteOilMarkAsync(int id)
        {

            var response = await _api.DeleteAsync($"oilMarks/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete oilMark with id: {id}.");
            }
        }

    }
}
