using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Cars
{
    public interface ICarDataStore
    {
        Task<List<Car>> GetCars();
        Task<Car> GetCar(int id);
        Task<Car> CreateCar(Car car);
        Task<Car> UpdateCar(int id, Car car);
        Task DeleteCar(int id);
    }
}
