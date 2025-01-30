using CheckDrive.Web.Requests.Cars;
using CheckDrive.Web.Services;
using CheckDrive.Web.ViewModels.Car;

namespace CheckDrive.Web.Stores.Cars;

internal sealed class CarStore(CheckDriveApi apiClient) : ICarStore
{
    private static readonly string resourceUrl = "cars";

    public Task<List<CarViewModel>> GetAsync()
        => apiClient.GetAsync<List<CarViewModel>>(resourceUrl);

    public Task<CarViewModel> GetByIdAsync(int id)
        => apiClient.GetAsync<CarViewModel>($"{resourceUrl}/{id}");

    public Task<CarViewModel> CreateAsync(CreateCarRequest request)
        => apiClient.PostAsync<CreateCarRequest, CarViewModel>(resourceUrl, request);

    public Task UpdateAsync(UpdateCarRequest request)
        => apiClient.PutAsync($"{resourceUrl}/{request.Id}", request);

    public Task DeleteAsync(int id)
        => apiClient.DeleteAsync($"{resourceUrl}/{id}");
}