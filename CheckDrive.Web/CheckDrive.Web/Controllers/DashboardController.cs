using CheckDrive.Web.Stores.Dashbord;
using CheckDrive.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardStore _store;
        public DashboardController(IDashboardStore store)
        {
            _store = store;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = await _store.GetDashboard();

            if (dashboard is null)
            {
                return BadRequest();
            }
            SetViewBagProperties(dashboard);

            return View();
        }
        private void SetViewBagProperties(DashboardViewModel dashboard)
        {
            var summary = dashboard.Summary;

            ViewBag.MonthlyFuelConsumption = summary.MonthlyFuelConsumption.ToString("0.00");
            ViewBag.CarsCount = summary.CarsCount;
            ViewBag.DriversCount = summary.DriversCount;

            ViewBag.EmployeesCountByRole = dashboard.EmployeesCountByRoles;
            ViewBag.SplineChartData = dashboard.SplineCharts;
        }
    }
}
