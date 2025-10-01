using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Services;
using Microsoft.AspNetCore.Authorization;
using proyecto_inmobiliaria.Constants;

namespace proyecto_inmobiliaria.Controllers
{
    [Authorize(Roles = Roles.Administrador)]
    public class AuditoriaController : Controller
    {
        private readonly IAuditoriaContratoService _auditoriaContratoService;
        private readonly IAuditoriaPagoService _auditoriaPagoService;

        public AuditoriaController(
            IAuditoriaContratoService auditoriaContratoService,
            IAuditoriaPagoService auditoriaPagoService)
        {
            _auditoriaContratoService = auditoriaContratoService;
            _auditoriaPagoService = auditoriaPagoService;
        }

        public IActionResult Index(
            string tipoAuditoria = "",
            int paginaNro = 1,
            int tamPagina = 10)
        {
            ViewData["TipoAuditoria"] = tipoAuditoria;

            IList<object> auditorias;
            int totalAuditorias;

            if (tipoAuditoria == "Contrato")
            {
                auditorias = _auditoriaContratoService.ListarAuditoria(paginaNro, tamPagina).Cast<object>().ToList();
                totalAuditorias = _auditoriaContratoService.CantidadAuditoria();
            }
            else if (tipoAuditoria == "Pago")
            {
                auditorias = _auditoriaPagoService.ListarAuditoria(paginaNro, tamPagina).Cast<object>().ToList();
                totalAuditorias = _auditoriaPagoService.CantidadAuditoria();
            }
            else
            {
                auditorias = new List<object>();
                totalAuditorias = 0;
            }

            int totalPaginas = (int)Math.Ceiling((double)totalAuditorias / tamPagina);
            ViewData["PaginaActual"] = paginaNro;
            ViewData["TotalPaginas"] = totalPaginas;

            return View(auditorias);
        }
    }
}
