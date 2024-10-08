using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Dashbord;
using CheckDrive.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CheckDrive.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardStore _store;
        private readonly IAccountDataStore _accountDataStore;
        public DashboardController(IDashboardStore store, IAccountDataStore accountDataStore)
        {
            _store = store;
            _accountDataStore = accountDataStore;
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
            var Debts = Newtonsoft.Json.JsonConvert.SerializeObject(dashboard.Debts);

            ViewBag.MonthlyFuelConsumption = summary.MonthlyFuelConsumption.ToString("0.00");
            ViewBag.CarsCount = summary.CarsCount;
            ViewBag.DriversCount = summary.DriversCount;

            ViewBag.EmployeesCountByRole = dashboard.EmployeesCountByRoles;
            ViewBag.SplineChartData = dashboard.SplineCharts;
            ViewBag.PetrolCount=dashboard.PetrolCounts;
            ViewBag.Debts = Debts;
        }
    }
}
