using CheckDrive.Web.Constants;
using CheckDrive.Web.Models;
using CheckDrive.Web.Stores.User;
using CheckDrive.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var roleId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            var accountId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            switch (roleId)
            {
                case "1":
                    return RedirectToAction("Index", "Dashboard");
                    break;
                case "3":
                    TempData["AccountId"] = accountId;
                    return RedirectToAction("Index", "PersonalDoctorReviews");
                    break;
                case "4":
                    return RedirectToAction("Index", "PersonalOperatorReviews");
                    break;
                case "5":
                    return RedirectToAction("PersonalIndex", "MechanicHandovers");
                    break;
                case "6":
                    return RedirectToAction("Index", "Dashboard");
                    break;
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ModelState.AddModelError("Password", "Incorrect password or login");
            return View(loginViewModel);
        }
    }
}
