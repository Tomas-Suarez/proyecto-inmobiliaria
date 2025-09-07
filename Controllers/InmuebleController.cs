using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Services;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace proyecto_inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IInmuebleService _service;
        private readonly IEstadoInmuebleRepository _estadoRepo;
        private readonly ITipoInmuebleRepository _tipoRepo;
        private readonly IPropietarioRepository _propietarioRepo;

        public InmuebleController(
            IInmuebleService service,
            ITipoInmuebleRepository tipoInmueble,
            IEstadoInmuebleRepository estadoInmueble,
            IPropietarioRepository propietarioRepo)
        {
            _service = service;
            _tipoRepo = tipoInmueble;
            _estadoRepo = estadoInmueble;
            _propietarioRepo = propietarioRepo;
        }

        public IActionResult Index(int paginaNro = 1, int tamPagina = 10, int? estado = null)
        {
            ViewData["ActivePage"] = "Inmueble";

            var inmuebles = _service.TodosLosInmueblesPaginados(paginaNro, tamPagina, estado);
            int totalInmuebles = _service.CantidadTotalInmuebles(estado);
            int totalPaginas = (int)Math.Ceiling((double)totalInmuebles / tamPagina);

            ViewData["PaginaActual"] = paginaNro;
            ViewData["TotalPaginas"] = totalPaginas;
            ViewData["EstadoSeleccionado"] = estado;

            var estados = _estadoRepo.ObtenerEstadoInmueble();
            ViewBag.EstadosInmueble = new SelectList(estados, "IdEstadoInmueble", "Descripcion", estado);


            return View(inmuebles);
        }

        [HttpGet]
        public IActionResult Crear(int id = 0)
        {
            var dto = new InmuebleRequestDTO(id, 0, 0, 0, "", 0, 0);

            CargarLista();
            return View("formCrearModificar", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(InmuebleRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                CargarLista();
                return View("formCrearModificar", dto);
            }
            _service.AltaInmueble(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Modificar(int IdInmueble)
        {
            var dto = _service.ObtenerRequestPorId(IdInmueble);

            CargarLista();
            return View("formCrearModificar", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modificar(InmuebleRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                CargarLista();
                return View("formCrearModificar", dto);
            }

            _service.ModificarInmueble(dto.IdInmueble, dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            _service.BajaInmueble(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult FiltrarPorDireccion(string direccion)
        {
            var inmuebles = _service.BuscarPorDireccion(direccion);
            return Json(inmuebles);
        }

        private void CargarLista()
        {
            var estados = _estadoRepo.ObtenerEstadoInmueble();
            ViewBag.EstadosInmueble = new SelectList(estados, "IdEstadoInmueble", "Descripcion");

            var tipos = _tipoRepo.ObtenerTipoInmueble();
            ViewBag.TiposInmueble = new SelectList(tipos, "IdTipoInmueble", "Nombre");
        }

        [HttpGet]
        public IActionResult Detalles(int IdInmueble)
        {
            var dto = _service.ObtenerPorId(IdInmueble);

            return View(dto);
        }
    }
}