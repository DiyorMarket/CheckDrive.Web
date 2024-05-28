using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.Drivers
{
    public class DriverDataStore : IDriverDataStore
    {
        private readonly ApiClient _api;

        public DriverDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetDriverResponse> GetDriversAsync()
        {
            var response = await _api.GetAsync("drivers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetDriverResponse>(json);

            return result;
        }

        public async Task<Driver> GetDriverAsync(int id)
        {
            var response = await _api.GetAsync($"drivers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch driver with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Driver>(json);

            return result;
        }

        public async Task<Driver> CreateDriverAsync(Driver driver)
        {
            var json = JsonConvert.SerializeObject(driver);
            var response = await _api.PostAsync("drivers", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating driver.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Driver>(jsonResponse);
        }

        public async Task<Driver> UpdateDriverAsync(int id, Driver driver)
        {
            var json = JsonConvert.SerializeObject(driver);
            var response = await _api.PutAsync($"drivers/{driver.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating driver.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Driver>(jsonResponse);
        }

        public async Task DeleteDriverAsync(int id)
        {
            var response = await _api.DeleteAsync($"drivers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete driver with id: {id}.");
            }
        }
    }
}
