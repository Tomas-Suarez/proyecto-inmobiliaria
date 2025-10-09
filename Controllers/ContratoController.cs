using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Services;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Dtos.response;
using Microsoft.AspNetCore.Authorization;
using proyecto_inmobiliaria.Constants;
using System.Security.Claims;


namespace proyecto_inmobiliaria.Controllers
{
    [Authorize]
    public class ContratoController : Controller
    {
        private readonly IContratoService _contratoService;
        public ContratoController(IContratoService service)
        {
            _contratoService = service;
        }

        public IActionResult Index(
            int paginaNro = 1,
            int tamPagina = 10,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            ViewData["ActivePage"] = "Contrato";
            ViewData["FechaDesde"] = fechaDesde;
            ViewData["FechaHasta"] = fechaHasta;

            IList<ContratoResponseDTO> contratos;
            int totalContratos;

            if (fechaDesde.HasValue && fechaHasta.HasValue)
            {
                contratos = _contratoService.ObtenerContratosVigentesPorFecha(fechaDesde.Value, fechaHasta.Value, paginaNro, tamPagina);
                totalContratos = _contratoService.CantidadContratosVigentesPorFecha(fechaDesde.Value, fechaHasta.Value);
            }
            else
            {
                contratos = _contratoService.TodosLosContratosPaginados(paginaNro, tamPagina);
                totalContratos = _contratoService.CantidadTotalContrato();
            }

            int totalPaginas = (int)Math.Ceiling((double)totalContratos / tamPagina);
            ViewData["PaginaActual"] = paginaNro;
            ViewData["TotalPaginas"] = totalPaginas;

            return View(contratos);
        }


        [HttpGet]
        public IActionResult Crear(int id = 0)
        {
            var dto = new ContratoRequestDTO(id, 0, 0, 0, DateTime.Today, DateTime.Today, null, false);
            return View("formCrearModificar", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(ContratoRequestDTO dto)
        {
            if (dto.IdInquilino == 0)
            {
                TempData["ErrorMensaje"] = "Faltó buscar y seleccionar el inquilino.";

                return View("formCrearModificar", dto);
            }

            if (_contratoService.ExisteSuperposicion(dto.IdInmueble, dto.FechaDesde, dto.FechaHasta))
            {
                ModelState.AddModelError("", "Las fechas seleccionadas se superponen con un contrato existente para este inmueble.");
            }

            if (!ModelState.IsValid)
            {
                return View("formCrearModificar", dto);
            }

            var claimSub = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            int idUsuario = int.Parse(claimSub!.Value);
            _contratoService.AltaContrato(dto, idUsuario);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Modificar(int IdContrato)
        {
            var dto = _contratoService.ObtenerRequestPorId(IdContrato);

            return View("formCrearModificar", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modificar(ContratoRequestDTO dto)
        {

            if (_contratoService.ExisteSuperposicion(dto.IdInmueble, dto.FechaDesde, dto.FechaHasta, dto.IdContrato))
            {
                ModelState.AddModelError("", "Las fechas seleccionadas se superponen con un contrato existente para este inmueble.");
            }
            
            if (!ModelState.IsValid)
            {
                return View("formCrearModificar", dto);
            }
            var claimSub = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            int idUsuario = int.Parse(claimSub!.Value);

            _contratoService.ModificarContrato(dto.IdContrato, dto, idUsuario, true);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Administrador)]
        public IActionResult Eliminar(int id)
        {
            _contratoService.BajaContrato(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Detalles(int IdContrato)
        {
            var dto = _contratoService.ObtenerPorId(IdContrato);

            return View(dto);
        }

        public IActionResult PorInmueble(int idInmueble, int paginaNro = 1, int tamPagina = 10)
        {
            var contratos = _contratoService.ContratosPorInmueble(idInmueble, paginaNro, tamPagina);
            int totalInmuebles = _contratoService.CantidadTotalPorInmueble(idInmueble);
            int totalPaginas = (int)Math.Ceiling((double)totalInmuebles / tamPagina);

            ViewData["PaginaActual"] = paginaNro;
            ViewData["TotalPaginas"] = totalPaginas;
            ViewData["IdInmueble"] = idInmueble;

            return View("ContratoPorInmueble", contratos);
        }

        public IActionResult FinalizarContratoAnticipado(int idContrato)
        {
            try
            {
                var pagoMulta = _contratoService.FinalizarContratoAnticipado(idContrato);
                return RedirectToAction("Crear", "Pago", new { idContrato, esFinalizacion = true });
            }
            catch (ContractAlreadyFinalizedException ex)
            {
                TempData["ErrorMensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (PendingPaymentsException ex)
            {
                TempData["ErrorMensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Renovar(int idContrato)
        {
            var contratoOriginal = _contratoService.ObtenerRequestPorId(idContrato);

            var responseContrato = _contratoService.ObtenerPorId(idContrato);

            var NuevoContrato = new ContratoRequestDTO(
                0,
                contratoOriginal.IdInquilino,
                contratoOriginal.IdInmueble,
                contratoOriginal.Monto,
                DateTime.Today,
                DateTime.Today,
                null,
                false
            );

            ViewData["NombreInquilino"] = responseContrato.NombreCompletoInquilino;
            ViewData["DireccionInmueble"] = responseContrato.DireccionInmueble;

            return View("formCrearModificar", NuevoContrato);
        }

    }
}