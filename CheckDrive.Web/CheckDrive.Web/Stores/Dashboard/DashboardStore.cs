using CheckDrive.Web.Services;
using CheckDrive.Web.ViewModels.Dashboard;

namespace CheckDrive.Web.Stores.Dashboard;

internal sealed class DashboardStore(CheckDriveApi apiClient) : IDashboardStore
{
    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var result = await apiClient.GetAsync<DashboardViewModel>("Dashboard");

        return result;
    }
}
