using CheckDrive.Web.ViewModels.Dashboard;

namespace CheckDrive.Web.Stores.MockDashboard
{
    public interface IMockDashboardStore
    {
        public Task<DashboardViewModel> GetDashboard();
    }
}
