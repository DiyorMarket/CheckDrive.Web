using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers.Reviews;
public class MechanicAcceptanceReview : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
