using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
