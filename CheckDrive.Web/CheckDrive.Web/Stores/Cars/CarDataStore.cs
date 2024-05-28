using CheckDrive.ApiContracts.Car;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.Cars
{
    public class CarDataStore : ICarDataStore
    {
        private readonly ApiClient _api;

        public CarDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }

        public async Task<GetCarResponse> GetCars()
        {
            var response = _api.Get("cars");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch cars.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetCarResponse>(json);

            return result;
        }

        public async Task<CarDto> GetCar(int id)
        {
            var response = _api.Get($"cars/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch car with id: {id}.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<CarDto>(json);

            return result;
        }

        public async Task<CarDto> CreateCar(CarForCreateDto carForCreate)
        {
            var json = JsonConvert.SerializeObject(carForCreate);
            var response = _api.Post("cars", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error creating cars.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<CarDto>(jsonResponse);
        }

        public async Task<CarDto> UpdateCar(int id, CarForUpdateDto car)
        {
            var json = JsonConvert.SerializeObject(car);
            var response = _api.Put($"cars/{car.Id}", json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating cars.");
            }

            var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<CarDto>(jsonResponse);
        }

        public async Task DeleteCar(int id)
        {
            var response = _api.Delete($"cars/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not delete car with id: {id}.");
            }
        }
    }
}
