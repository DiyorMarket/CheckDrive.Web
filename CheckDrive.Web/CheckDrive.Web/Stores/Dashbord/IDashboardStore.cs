using CheckDrive.Web.ViewModels.Dashboard;

namespace CheckDrive.Web.Stores.Dashbord;

public interface IDashboardStore
{
    public Task<DashboardViewModel> GetDashboardAsync();
}
