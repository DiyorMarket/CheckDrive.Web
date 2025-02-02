using CheckDrive.Web.Requests.Cars;
using CheckDrive.Web.ViewModels.Car;

namespace CheckDrive.Web.Stores.Cars;

public interface ICarStore
{
    Task<List<CarViewModel>> GetAsync();
    Task<CarViewModel> GetByIdAsync(int id);
    Task<CarViewModel> CreateAsync(CreateCarRequest request);
    Task UpdateAsync(UpdateCarRequest request);
    Task DeleteAsync(int id);
}
