using CheckDrive.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CheckDrive.Web.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ApiException exception)
            {
                int statusCode = (int)exception.StatusCode;
                context.Result = new RedirectToActionResult("ErrorPage", "Error", new { statusCode });
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new RedirectToActionResult("ErrorPage", "Error", new { statusCode = 500 });
                context.ExceptionHandled = true;
            }
        }
    }
}
