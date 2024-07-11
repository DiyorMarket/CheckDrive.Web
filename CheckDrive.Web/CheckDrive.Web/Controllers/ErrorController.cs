using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorPage(int statusCode)
        {
            return statusCode switch
            {
                401 => RedirectToAction(nameof(Unauthorized)),
                403 => RedirectToAction(nameof(Forbidden)),
                404 => RedirectToAction(nameof(NotFound)),
                409 => RedirectToAction(nameof(Error409)),
                _ => RedirectToAction(nameof(InternalServerError)),
            };
        }

        public IActionResult Unauthorized()
        {
            return View();
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        public IActionResult NotFound()
        {
            return View();
        }

        public IActionResult InternalServerError()
        {
            return View();
        }
        public IActionResult Error409()
        {
            return View();
        }
    }
}
