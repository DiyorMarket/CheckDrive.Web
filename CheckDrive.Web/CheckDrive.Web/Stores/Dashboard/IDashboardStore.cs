using CheckDrive.Web.ViewModels.Dashboard;

namespace CheckDrive.Web.Stores.Dashboard;

public interface IDashboardStore
{
    public Task<DashboardViewModel> GetDashboardAsync();
}
