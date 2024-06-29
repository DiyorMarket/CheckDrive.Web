using CheckDrive.ApiContracts.Car;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Text;

namespace CheckDrive.Web.Stores.Cars
{
    public class CarDataStore : ICarDataStore
    {
        private readonly ApiClient _api;

        public CarDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetCarResponse> GetCarsAsync(string? searchString,int? pageNumber)

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

            var response = await _api.GetAsync("cars?" + query.ToString());

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch cars.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetCarResponse>(json);

            return result;
        }

        public async Task<CarDto> GetCarAsync(int id)
        {
            var response = await _api.GetAsync($"cars/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch car with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<CarDto>(json);

            return result;
        }

        public async Task<CarDto> CreateCarAsync(CarForCreateDto carForCreate)
        {
            var json = JsonConvert.SerializeObject(carForCreate);
            var response = await _api.PostAsync("cars", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating cars.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<CarDto>(jsonResponse);
        }

        public async Task<CarDto> UpdateCarAsync(int id, CarForUpdateDto car)
        {
            var json = JsonConvert.SerializeObject(car);
            var response = await _api.PutAsync($"cars/{car.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating cars.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<CarDto>(jsonResponse);
        }

        public async Task DeleteCarAsync(int id)
        {
            var response = await _api.DeleteAsync($"cars/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete car with id: {id}.");
            }
        }
    }
}
