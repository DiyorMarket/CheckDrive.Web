using CheckDrive.Web.Requests.Auth;
using CheckDrive.Web.Stores.Auth;
using CheckDrive.Web.Stores.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers;

public class HomeController(IAuthStore authStore, IDashboardStore dashboardStore) : Controller
{
    public async Task<IActionResult> Index()
    {
        var result = await dashboardStore.GetDashboardAsync();

        return View(result);
    }

    [HttpGet, Route("login")]
    public IActionResult Login() => View();

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        await authStore.LoginAsync(request);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet, Route("logout")]
    public IActionResult Logout()
    {
        authStore.Logout();

        return RedirectToAction(nameof(Login));
    }
}
