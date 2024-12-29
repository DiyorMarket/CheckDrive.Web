using CheckDrive.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CheckDrive.Web.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var ex = context.Exception;
        _logger.LogError(
            context.Exception,
            "Unhandled HTTP exception occurred, {@Message}",
            ex.Message);

        if (context.Exception is ApiException exception)
        {
            int statusCode = (int)exception.StatusCode;
            context.Result = new RedirectToActionResult("ErrorPage", "Error", new { statusCode });
            context.ExceptionHandled = true;
        }
        else if (context.Exception is HttpRequestException error)
        {
            context.Result = new RedirectToActionResult("ErrorPage", "Error", new { error.StatusCode });
            context.ExceptionHandled = true;
        }
        else
        {
            context.Result = new RedirectToActionResult("ErrorPage", "Error", new { statusCode = 500 });
            context.ExceptionHandled = true;
        }
    }
}
