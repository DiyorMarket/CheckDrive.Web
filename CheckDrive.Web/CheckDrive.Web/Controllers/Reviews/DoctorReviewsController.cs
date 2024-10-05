using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers.Reviews;
public class DoctorReviewsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
