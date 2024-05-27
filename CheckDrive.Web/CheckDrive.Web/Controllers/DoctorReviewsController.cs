using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DoctorReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
