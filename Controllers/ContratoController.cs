using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Services;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IContratoService _service;

        public ContratoController(IContratoService service)
        {
            _service = service;
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
                contratos = _service.ObtenerContratosVigentesPorFecha(fechaDesde.Value, fechaHasta.Value, paginaNro, tamPagina);
                totalContratos = _service.CantidadContratosVigentesPorFecha(fechaDesde.Value, fechaHasta.Value);
            }
            else
            {
                contratos = _service.TodosLosContratosPaginados(paginaNro, tamPagina);
                totalContratos = _service.CantidadTotalContrato();
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
            if (!ModelState.IsValid)
            {
                return View("formCrearModificar", dto);
            }
            try
            {
                _service.AltaContrato(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (ContractOverlapException ex)
            {
                TempData["ErrorMensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Modificar(int IdContrato)
        {
            var dto = _service.ObtenerRequestPorId(IdContrato);

            return View("formCrearModificar", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modificar(ContratoRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("formCrearModificar", dto);
            }
            _service.ModificarContrato(dto.IdContrato, dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            _service.BajaContrato(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Detalles(int IdContrato)
        {
            var dto = _service.ObtenerPorId(IdContrato);

            return View(dto);
        }

        public IActionResult PorInmueble(int idInmueble, int paginaNro = 1, int tamPagina = 10)
        {
            var contratos = _service.ContratosPorInmueble(idInmueble, paginaNro, tamPagina);
            int totalInmuebles = _service.CantidadTotalPorInmueble(idInmueble);
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
                var pagoMulta = _service.FinalizarContratoAnticipado(idContrato);
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
            var contratoOriginal = _service.ObtenerRequestPorId(idContrato);

            var responseContrato = _service.ObtenerPorId(idContrato);

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