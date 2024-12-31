using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

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
        var exception = context.Exception;

        _logger.LogError(
            context.Exception,
            "Unhandled HTTP exception occurred, {@Message}",
            exception.Message);

        var statusCode = GetStatusCode(exception);
        context.Result = new RedirectToActionResult("ErrorPage", "Error", new { statusCode });
        context.ExceptionHandled = true;
    }

    private static int GetStatusCode(Exception ex) =>
        ex switch
        {
            HttpRequestException apiException => apiException.StatusCode.HasValue ? (int)apiException.StatusCode.Value : 500,
            SecurityTokenException => StatusCodes.Status401Unauthorized,
            _ => 500
        };
}
