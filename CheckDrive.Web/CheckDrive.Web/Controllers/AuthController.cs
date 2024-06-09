using CheckDrive.Web.Constants;
using CheckDrive.Web.Stores.User;
using CheckDrive.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserDataStore _userDataStore;
        public AuthController(IUserDataStore userDataStore)
        {
            _userDataStore = userDataStore;
        }
        public IActionResult Login()
        {
            HttpContext.Response.Cookies.Delete("JwtToken");
            return RedirectToAction("Index", "Auth");
        }
        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.TryGetValue(Configurations.JwtToken, out _))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = new LoginViewModel
            {
                Login = loginViewModel.Login,
                Password = loginViewModel.Password,
            };

            var (success, token) = await _userDataStore.AuthenticateLoginAsync(user);

            if (success)
            {
                HttpContext.Response.Cookies.Append("tasty-cookies", token, new CookieOptions
                {
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = true,
                    IsEssential = true
                });
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ModelState.AddModelError("Password", "Incorrect password or login");
            return View(loginViewModel);
        }
    }
}
