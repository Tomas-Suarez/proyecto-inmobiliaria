using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Services;

namespace proyecto_inmobiliaria.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly IPropietarioService service;

        public PropietarioController(IPropietarioService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Propietario";
            IList<PropietarioResponseDTO> propietarios = service.TodosLosPropietarios();
            return View(propietarios);
        }

        [HttpGet]
        public IActionResult Crear(int id = 0)
        {
            var dto = new PropietarioRequestDTO(id, "", "", "", "", "", "");
            return View("formCrearModificar", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(PropietarioRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("formCrearModificar", dto);
            }
            service.AltaPropietario(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Modificar(int IdPropietario)
        {
            var propietario = service.ObtenerPorId(IdPropietario);

            var dto = new PropietarioRequestDTO(
                IdPropietario,
                propietario.Nombre!,
                propietario.Apellido!,
                propietario.Documento!,
                propietario.Telefono!,
                propietario.Email!,
                propietario.Direccion!
            );

            return View("formCrearModificar", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modificar(PropietarioRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("formCrearModificar", dto);
            }
            service.ModificarPropietario(dto.IdPropietario, dto);
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            service.BajaPropietario(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult BuscarPorDocumento(string documento)
        {
            try
            {
                var propietario = service.BuscarPorDocumento(documento);
                return Json(new { success = true, data = propietario });

            }
            catch (NotFoundException)
            {
                return Ok(new { success = false, message = "No se encontró propietario con ese documento." });
            }
        }

        [HttpGet]
        public IActionResult Detalles(int IdPropietario)
        {
            var dto = service.ObtenerPorId(IdPropietario);

            return View(dto);
        } 

    }
}