using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Services;
using proyecto_inmobiliaria.Dtos.response;
using System.Linq;
using System.Collections.Generic;

namespace proyecto_inmobiliaria.Controllers
{
    public class AuditoriaController : Controller
    {
        private readonly IAuditoriaContratoService _auditoriaContratoService;
        //private readonly IAuditoriaPagoService _auditoriaPagoService;

        public AuditoriaController(
            IAuditoriaContratoService auditoriaContratoService)
        {
            _auditoriaContratoService = auditoriaContratoService;
            //_auditoriaPagoService = auditoriaPagoService;
        }

        // GET: /Auditoria
        public IActionResult Index(string tipoAuditoria = "")
        {
            ViewData["TipoAuditoria"] = tipoAuditoria;

            if (tipoAuditoria == "Contrato")
            {
                var auditorias = _auditoriaContratoService.ListarAuditoria(1, 100);
                return View(auditorias.Cast<object>());
            }
            else
            {
                // Si no se selecciona tipo, mostrar tabla vacía
                return View(Enumerable.Empty<object>());
            }
        }
    }
}
