using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Exceptions;
using static proyecto_inmobiliaria.Constants.GenericConstants;

namespace proyecto_inmobiliaria.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(int? statusCode = null)
        {
            // Nos sirve para obtener la información de la excepcion capturada
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            int status = statusCode ?? 500;
            string message = ERROR_INESPERADO;

            if (exceptionHandlerFeature != null) // Nos da null, si alguien accede directamente a /error
            {
                message = exceptionHandlerFeature.Error.Message;

                if (exceptionHandlerFeature.Error is CustomException customException)
                    status = customException.Status;
            }
            else if (status == 404)
            {
                message = PAGINA_NO_ENCONTRADA;
            }

            ViewData["Status"] = status;
            ViewData["Message"] = message;

            return View("errorPage");
        }
    }
}
