using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers.Reviews;
public class OperatorReviewsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
