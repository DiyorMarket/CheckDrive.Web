﻿using CheckDrive.ApiContracts.Account;
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
            HttpContext.Response.Cookies.Delete("tasty-cookies");
            return RedirectToAction("Index", "Auth");
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.TryGetValue("tasty-cookies", out _))
            {
                string token = HttpContext.Request.Cookies["tasty-cookies"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var roleId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

                switch (roleId)
                {
                    case "1":
                        return RedirectToAction("Index", "Dashboard");
                    case "3":
                        return RedirectToAction("PersonalIndex", "DoctorReviews");
                    case "4":
                        return RedirectToAction("PersonalIndex", "OperatorReviews");
                    case "5":
                        return RedirectToAction("PersonalIndex", "DispatcherReviews");
                    case "6":
                        return RedirectToAction("PersonalIndex", "MechanicHandovers");
                }
                return RedirectToAction("Index", "Auth");
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Index(AccountForLoginDto loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = new AccountForLoginDto
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
                    case "3":
                        TempData["AccountId"] = accountId;
                        return RedirectToAction("PersonalIndex", "DoctorReviews");
                    case "4":
                        TempData["AccountId"] = accountId;
                        return RedirectToAction("PersonalIndex", "OperatorReviews");
                    case "5":
                        TempData["AccountId"] = accountId;
                        return RedirectToAction("PersonalIndex", "DispatcherReviews");
                    case "6":
                        TempData["AccountId"] = accountId;
                        return RedirectToAction("PersonalIndex", "MechanicHandovers");
                    default:
                        return RedirectToAction("Index", "Auth");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ModelState.AddModelError("Password", "Incorrect password or login");
            return View(loginViewModel);
        }
    }
}
