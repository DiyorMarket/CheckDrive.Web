using CheckDrive.ApiContracts.Car;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Cars
{
    public interface ICarDataStore
    {
        Task<GetCarResponse> GetCarsAsync(string? searchString, int? pageNumber);
        Task<GetCarResponse> GetCarsAsync(int? roleId, bool? isBusy);
        Task<CarDto> GetCarAsync(int id);
        Task<GetCarHistoryResponse> GetCarsHistoryAsync(string? searchString, int? pageNumber, int? year, int? month);
        Task<CarDto> CreateCarAsync(CarForCreateDto carForCreate);
        Task<CarDto> UpdateCarAsync(int id, CarForUpdateDto car);
        Task DeleteCarAsync(int id);
    }
}
