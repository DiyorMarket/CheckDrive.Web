using CheckDrive.ApiContracts.Car;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;
using System.Security.Principal;

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

        public async Task<Car> UpdateCar(int id, Car car)
        {
            await Task.Delay(100);
            var existingCar = _cars.FirstOrDefault(c => c.Id == id);
            if (existingCar != null)
            {
                existingCar.Model = car.Model;
                existingCar.Color = car.Color;
                existingCar.Number = car.Number;
                existingCar.MeduimFuelConsumption = car.MeduimFuelConsumption;
                existingCar.FuelTankCapacity = car.FuelTankCapacity;
                existingCar.ManufacturedYear = car.ManufacturedYear;
            }
            return existingCar;
        }

        public async Task DeleteCar(int id)
        {
            await Task.Delay(100);
            var carToRemove = _cars.FirstOrDefault(c => c.Id == id);
            if (carToRemove != null)
            {
                _cars.Remove(carToRemove);
            }
        }
    }
}
