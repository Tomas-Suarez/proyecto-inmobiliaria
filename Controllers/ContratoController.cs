using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Repository.imp;
using proyecto_inmobiliaria.Services;

namespace proyecto_inmobiliaria.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IContratoService _service;

        public ContratoController(IContratoService service)
        {
            _service = service;
        }

        public IActionResult Index(int paginaNro = 1, int tamPagina = 10)
        {
            ViewData["ActivePage"] = "Contrato";

            var contratos = _service.TodosLosContratosPaginados(paginaNro, tamPagina);
            int totalInmuebles = _service.CantidadTotalContrato();
            int totalPaginas = (int)Math.Ceiling((double)totalInmuebles / tamPagina);

            ViewData["TotalPaginas"] = totalPaginas;

            return View(contratos);
        }

        [HttpGet]
        public IActionResult Crear(int id = 0)
        {
            var dto = new ContratoRequestDTO(id, 0, 0, 0, null, null);
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
            _service.AltaContrato(dto);
            return RedirectToAction(nameof(Index));
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
           var hola = _service.ModificarContrato(dto.IdContrato, dto);

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

    }
}