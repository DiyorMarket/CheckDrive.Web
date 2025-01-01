using CheckDrive.Web.Requests.Employee;
using CheckDrive.Web.Services;
using CheckDrive.Web.ViewModels.Employee;

namespace CheckDrive.Web.Stores.Employee;

internal sealed class EmployeeStore : IEmployeeStore
{
    private static readonly string resourceUrl = "employees";
    private readonly CheckDriveApi _apiClient;

    public EmployeeStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public Task<List<EmployeeViewModel>> GetAsync()
        => _apiClient.GetAsync<List<EmployeeViewModel>>(resourceUrl);

    public Task<EmployeeViewModel> GetByIdAsync(int id)
        => _apiClient.GetAsync<EmployeeViewModel>($"{resourceUrl}/{id}");

    public Task<EmployeeViewModel> CreateAsync(CreateEmployeeRequest request)
        => _apiClient.PostAsync<CreateEmployeeRequest, EmployeeViewModel>(resourceUrl, request);

    public Task UpdateAsync(UpdateEmployeeRequest request)
        => _apiClient.PutAsync($"{resourceUrl}/{request.Id}", request);

    public Task DeleteAsync(int id)
        => _apiClient.DeleteAsync($"{resourceUrl}/{id}");
}
