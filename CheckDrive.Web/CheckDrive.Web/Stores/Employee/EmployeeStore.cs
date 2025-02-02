using CheckDrive.Web.Requests.Employee;
using CheckDrive.Web.Services;
using CheckDrive.Web.ViewModels.Employee;

namespace CheckDrive.Web.Stores.Employee;

internal sealed class EmployeeStore(CheckDriveApi apiClient) : IEmployeeStore
{
    private static readonly string resourceUrl = "employees";

    public Task<List<EmployeeViewModel>> GetAsync()
        => apiClient.GetAsync<List<EmployeeViewModel>>(resourceUrl);

    public Task<EmployeeViewModel> GetByIdAsync(int id)
        => apiClient.GetAsync<EmployeeViewModel>($"{resourceUrl}/{id}");

    public Task<EmployeeViewModel> CreateAsync(CreateEmployeeRequest request)
        => apiClient.PostAsync<CreateEmployeeRequest, EmployeeViewModel>(resourceUrl, request);

    public Task UpdateAsync(UpdateEmployeeRequest request)
        => apiClient.PutAsync($"{resourceUrl}/{request.Id}", request);

    public Task DeleteAsync(int id)
        => apiClient.DeleteAsync($"{resourceUrl}/{id}");
}
