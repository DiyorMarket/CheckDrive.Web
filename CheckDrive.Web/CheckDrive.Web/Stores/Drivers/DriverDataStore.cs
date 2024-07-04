using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Driver;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.Drivers
{
    public class DriverDataStore : IDriverDataStore
    {
        private readonly ApiClient _api;

        public DriverDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetDriverResponse> GetDriversAsync(string? searchString, int? pageNumber)
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

            var response = await _api.GetAsync("drivers?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }


            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetDriverResponse>(json);

            return result;
        }
        public async Task<IEnumerable<DriverHistoryDto>> GetDriverHistories(int Id)
        {
            var response = await _api.GetAsync($"drivers/driverHistories?driverId={Id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<DriverHistoryDto>>(json);

            return result;
        }

        public async Task<DriverDto> GetDriverAsync(int id)
        {
            var response = await _api.GetAsync($"drivers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch driver with id: {id}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DriverDto>(json);

            return result;
        }

        public async Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate)
        {
            var json = JsonConvert.SerializeObject(driverForCreate);
            var response = await _api.PostAsync("drivers", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating driver.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DriverDto>(jsonResponse);
        }

        public async Task<DriverDto> UpdateDriverAsync(int id, AccountForUpdateDto driverForUpdate)
        {
            var json = JsonConvert.SerializeObject(driverForUpdate);
            var response = await _api.PutAsync($"drivers/{driverForUpdate.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating driver.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DriverDto>(jsonResponse);
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
