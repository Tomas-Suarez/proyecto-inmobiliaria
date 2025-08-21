using Exceptions;
using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
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
            try
            {
                IList<PropietarioResponseDTO> propietarios = service.TodosLosPropietarios();
                return View(propietarios);
            }
            catch (Exception ex) //TODO: crear un exception handler, para eviatr el try catch
            {
                ViewBag.Error = ex.Message;
                return View(new List<PropietarioResponseDTO>());
            }
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

            try
            {
                service.AltaPropietario(dto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("formCrearModificar", dto);
            }
        }

        [HttpGet]
        public IActionResult Modificar(int IdPropietario)
        {
            try
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
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modificar(PropietarioRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return View("formCrearModificar", dto);

            try
            {
                service.ModificarPropietario(dto.IdPropietario, dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("formCrearModificar", dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            try
            {
                service.BajaPropietario(id);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}