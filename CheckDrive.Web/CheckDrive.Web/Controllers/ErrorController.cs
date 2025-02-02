using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("Error/{statusCode:int}")]
    public IActionResult ErrorPage(int statusCode)
    {
        return statusCode switch
        {
            401 => View(nameof(Unauthorized)),
            403 => View(nameof(Forbidden)),
            404 => View(nameof(NotFound)),
            409 => RedirectToAction(nameof(Error409)),
            _ => View(nameof(InternalServerError)),
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
