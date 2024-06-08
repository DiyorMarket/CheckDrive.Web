using CheckDrive.Web.ViewModels;

namespace CheckDrive.Web.Stores.Dashbord
{
    public interface IDashboardStore
    {
        public Task<DashboardViewModel?> GetDashboard();
    }
}
