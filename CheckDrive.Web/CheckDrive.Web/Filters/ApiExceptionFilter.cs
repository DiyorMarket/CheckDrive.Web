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
                if (statusCode == 401)
                {
                    context.Result = new RedirectToActionResult("Index", "Auth", new { statusCode });
                }

                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new RedirectToActionResult("Error", "Home", new { statusCode = 500 });
                context.ExceptionHandled = true;
            }
        }
    }
}
