using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Cars
{
    public class MockCarDataStore : ICarDataStore
    {
        private readonly List<Car> _cars;

        public MockCarDataStore()
        {
            _cars = new List<Car>
            {
                new Car { Id = 1, Model = "Toyota Camry", Color = "Red", Number = "ABC123", MeduimFuelConsumption = 8.5, FuelTankCapacity = 60, ManufacturedYear = 2020 },
                new Car { Id = 2, Model = "Honda Civic", Color = "Blue", Number = "DEF456", MeduimFuelConsumption = 7.2, FuelTankCapacity = 55, ManufacturedYear = 2019 },
            };
        }

        public async Task<List<Car>> GetCars()
        {
            await Task.Delay(100); 
            return _cars.ToList();
        }

        public async Task<Car> GetCar(int id)
        {
            await Task.Delay(100);
            return _cars.FirstOrDefault(c => c.Id == id);
        }

        public async Task<Car> CreateCar(Car car)
        {
            await Task.Delay(100); 
            car.Id = _cars.Max(c => c.Id) + 1;
            _cars.Add(car);
            return car;
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
