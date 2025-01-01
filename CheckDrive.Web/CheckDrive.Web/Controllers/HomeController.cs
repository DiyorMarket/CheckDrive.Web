using CheckDrive.Web.Stores.Dashbord;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardStore _dashboardStore;

    public HomeController(IDashboardStore dashboardStore)
    {
        _dashboardStore = dashboardStore ?? throw new ArgumentNullException(nameof(dashboardStore));
    }

    public async Task<IActionResult> Index()
    {
        var result = await _dashboardStore.GetDashboardAsync();

        return View(result);
    }
}
