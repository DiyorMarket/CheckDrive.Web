using CheckDrive.Web.Requests.Auth;
using CheckDrive.Web.Stores.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers;

public class AuthController(IAuthStore authStore) : Controller
{
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        await authStore.LoginAsync(request);

        return RedirectToAction("Index", "Home");
    }
}

