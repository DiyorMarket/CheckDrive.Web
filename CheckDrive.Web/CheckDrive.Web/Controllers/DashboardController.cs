using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Dashbord;
using CheckDrive.Web.Stores.Debts;
using CheckDrive.Web.Stores.MockDashboard;
using CheckDrive.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CheckDrive.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardStore _store;
        private readonly IAccountDataStore _accountDataStore;
        private readonly IMockDashboardStore _mockDashboardStore;
        private readonly IDebtsStore _debtsStore;
        public DashboardController(IDashboardStore store, 
            IAccountDataStore accountDataStore,
            IMockDashboardStore mockDashboardStore,
            IDebtsStore debtsStore)
        {
            _store = store;
            _accountDataStore = accountDataStore;
            _mockDashboardStore = mockDashboardStore;
            _debtsStore = debtsStore;
        }

        public async Task<IActionResult> Index()
        {
            string token = HttpContext.Request.Cookies["tasty-cookies"];
         
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            
            if (jwtToken == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            var accountId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            int accountIds = Int32.Parse(accountId);

            TempData["UserName"] = _accountDataStore.GetAccountAsync(accountIds)
                .Result.FirstName;
            TempData.Keep("UserName");
            
            var dashboard = await _store.GetDashboard();
            var mockDashboard = await _mockDashboardStore.GetDashboard();
            var debts = _debtsStore.GetAll("" , "");


            dashboard.Debts=debts;
            dashboard.OilAmount=mockDashboard.OilAmount;

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
            ViewBag.OilAmount=dashboard.OilAmount;
            ViewBag.Debts = dashboard.Debts;
        }
    }
}
