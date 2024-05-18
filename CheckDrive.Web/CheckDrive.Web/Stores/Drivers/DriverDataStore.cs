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

        public async Task<GetDriverResponse> GetDrivers()
        {
            var response = _api.Get("drivers?");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetDriverResponse>(json);

            return result;
        }

        public async Task<Driver> GetDriver(int id)
        {
            var response = _api.Get($"drivers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch drivers with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Driver>(json);

            return result;
        }

        public async Task<Driver> CreateDriver(Driver driver)
        {
            var json = JsonConvert.SerializeObject(driver);
            var response = _api.Post("drivers", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating drivers.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<Driver>(jsonResponse);
        }

        public async Task<Driver> UpdateDriver(int id, Driver driver)
        {
            var json = JsonConvert.SerializeObject(driver);
            var response = _api.Put($"drivers/{driver.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating drivers.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<Driver>(jsonResponse);
        }

        public async Task DeleteDriver(int id)
        {
            var response = _api.Delete($"drivers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete drivers with id: {id}.");
            }
        }
    }
}
