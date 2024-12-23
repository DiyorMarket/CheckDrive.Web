using CheckDrive.Web.Service;
using CheckDrive.Web.ViewModels.Dashboard;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.Dashbord
{
    public class DashboardStore : IDashboardStore
    {
        private readonly ApiClient _client;
        public DashboardStore(ApiClient client)
        {
            _client = client;
        }
        public async Task<DashboardViewModel?> GetDashboard()
        {
            var response = await _client.GetAsync("Dashboard");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error occured while fetching dashboard data.");
            }
            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<DashboardViewModel>(json);
            return result;
        }
    }
}
