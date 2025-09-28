using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Services;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using proyecto_inmobiliaria.Dtos.response;

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

        public IActionResult Index(
            int paginaNro = 1,
            int tamPagina = 10,
            int? estado = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            ViewData["ActivePage"] = "Inmueble";
            ViewData["EstadoSeleccionado"] = estado;
            ViewData["FechaDesde"] = fechaDesde;
            ViewData["FechaHasta"] = fechaHasta;

            IList<InmuebleResponseDTO> inmuebles;
            int totalInmuebles;

            if (fechaDesde.HasValue && fechaHasta.HasValue)
            {
                inmuebles = _service.ObtenerDisponiblesPorFecha(fechaDesde.Value, fechaHasta.Value, paginaNro, tamPagina);
                totalInmuebles = _service.CantidadTotalDisponiblesPorFecha(fechaDesde.Value, fechaHasta.Value);
            }
            else if (estado.HasValue)
            {
                inmuebles = _service.TodosLosInmueblesPaginados(paginaNro, tamPagina, estado);
                totalInmuebles = _service.CantidadTotalInmuebles(estado);
            }
            else
            {
                inmuebles = _service.TodosLosInmueblesPaginados(paginaNro, tamPagina, null);
                totalInmuebles = _service.CantidadTotalInmuebles(null);
            }

            int totalPaginas = (int)Math.Ceiling((double)totalInmuebles / tamPagina);
            ViewData["PaginaActual"] = paginaNro;
            ViewData["TotalPaginas"] = totalPaginas;

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

        [HttpGet]
        public IActionResult PorPropietario(int IdPropietario, int paginaNro = 1, int tamPagina = 10)
        {
            var inmuebles = _service.ObtenerInmueblesPorPropietario(IdPropietario, paginaNro, tamPagina);
            int totalInmuebles = _service.CantidadTotalPorPropietario(IdPropietario);
            int totalPaginas = (int)Math.Ceiling((double)totalInmuebles / tamPagina);

            var propietario = _propietarioRepo.ObtenerPorId(IdPropietario);
            ViewBag.PropietarioNombre = $"{propietario.Nombre} {propietario.Apellido}";

            ViewData["PaginaActual"] = paginaNro;
            ViewData["TotalPaginas"] = totalPaginas;
            ViewData["IdPropietario"] = IdPropietario;

            return View("InmueblesPorPropietarios", inmuebles);
        }

    }
}