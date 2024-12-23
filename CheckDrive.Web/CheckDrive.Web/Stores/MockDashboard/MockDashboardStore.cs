using CheckDrive.Web.ViewModels.Dashboard;

namespace CheckDrive.Web.Stores.MockDashboard
{
    public class MockDashboardStore : IMockDashboardStore
    {
        public async Task<DashboardViewModel> GetDashboard()
        {
            var viewModel = new DashboardViewModel
            {
                OilAmount = GetMockOilAmounts()
            };

            return viewModel;
        }


        private List<OilMarkViewModel> GetMockOilAmounts()
        {
            return new List<OilMarkViewModel>()
            {
                new OilMarkViewModel() { Name = "Ai80", Amount = 15 },
                new OilMarkViewModel() { Name = "Ai91", Amount = 10 },
                new OilMarkViewModel() { Name = "Ai92", Amount = 2 },
                new OilMarkViewModel() { Name = "Ai95", Amount = 5 },
                new OilMarkViewModel() { Name = "Diesel", Amount = 9 }
            };
        }
    }
}
