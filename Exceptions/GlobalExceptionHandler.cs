using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace proyecto_inmobiliaria.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            int statusCode = exception is CustomException customException
                ? customException.Status
                : StatusCodes.Status500InternalServerError;

            httpContext.Response.StatusCode = statusCode;

            var tempDataFactory = httpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
            var tempData = tempDataFactory.GetTempData(httpContext);

            tempData["ErrorStatus"] = statusCode;
            tempData["ErrorMessage"] = exception.Message;

            httpContext.Response.Redirect("/Error");

            return ValueTask.FromResult(true);
        }
    }
}
