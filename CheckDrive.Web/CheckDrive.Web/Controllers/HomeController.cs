using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
