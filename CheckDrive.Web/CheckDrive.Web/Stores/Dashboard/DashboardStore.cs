using CheckDrive.Web.Services;
using CheckDrive.Web.ViewModels.Dashboard;

namespace CheckDrive.Web.Stores.Dashboard;

public class DashboardStore : IDashboardStore
{
    private readonly CheckDriveApi _apiClient;

    public DashboardStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var result = await _apiClient.GetAsync<DashboardViewModel>("Dashboard");

        return result;
    }
}
