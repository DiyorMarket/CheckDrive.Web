using CheckDrive.ApiContracts.Car;
using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.Cars;

public class CarDataStore : ICarDataStore
{
    private readonly CheckDriveApi _apiClient;

    public CarDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<List<Car>> GetAsync()
        => _apiClient.GetAsync<List<Car>>("cars");

    public Task<CarDto> CreateCarAsync(CarForCreateDto carForCreate)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCarAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CarDto> GetCarAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetCarResponse> GetCarsAsync(string? searchString, int? pageNumber)
    {
        throw new NotImplementedException();
    }

    public Task<GetCarResponse> GetCarsAsync(int? roleId, bool? isBusy)
    {
        throw new NotImplementedException();
    }

    public Task<GetCarHistoryResponse> GetCarsHistoryAsync(string? searchString, int? pageNumber, int? year, int? month)
    {
        throw new NotImplementedException();
    }

    public Task<CarDto> UpdateCarAsync(int id, CarForUpdateDto car)
    {
        throw new NotImplementedException();
    }

    public Task<DTOs.Car.CarDto> CreateCarAsync(DTOs.Car.CarForCreateDto carForCreate)
    {
        throw new NotImplementedException();
    }

    public Task<DTOs.Car.CarDto> UpdateCarAsync(int id, DTOs.Car.CarForUpdateDto car)
    {
        throw new NotImplementedException();
    }
}
