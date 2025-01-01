using CheckDrive.Web.ViewModels.Dashboard;

namespace CheckDrive.Web.Stores.MockDashboard
{
    public class MockDashboardStore : IMockDashboardStore
    {
        public async Task<DashboardViewModel> GetDashboard()
        {
            return new DashboardViewModel(null, null, null, null);
        }
    }
}
