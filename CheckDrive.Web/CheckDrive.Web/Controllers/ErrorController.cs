using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    public IActionResult Index() => View();

    [Route("{statusCode:int}")]
    public IActionResult ErrorPage(int statusCode)
    {
        return statusCode switch
        {
            401 => RedirectToAction("Login", "Home"),
            403 => View(nameof(Forbidden)),
            404 => View(nameof(NotFound)),
            409 => RedirectToAction(nameof(Error409)),
            _ => View(nameof(InternalServerError)),
        };
    }

    public IActionResult Unauthorized() => View();

    public IActionResult Forbidden() => View();

    public IActionResult NotFound() => View();

    public IActionResult InternalServerError() => View();

    public IActionResult Error409() => View();
}
