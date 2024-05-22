using CheckDrive.ApiContracts.Car;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Cars
{
    public interface ICarDataStore
    {
        Task<GetCarResponse> GetCars();
        Task<CarDto> GetCar(int id);
        Task<CarDto> CreateCar(CarForCreateDto carForCreate);
        Task<CarDto> UpdateCar(int id, CarForUpdateDto car);
        Task DeleteCar(int id);
    }
}
