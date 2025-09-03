using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Services;

namespace proyecto_inmobiliaria.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly IInquilinoService service;

        public InquilinoController(IInquilinoService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Inquilino";
            IList<InquilinoResponseDTO> inquilinos = service.TodosLosInquilinos();
            return View(inquilinos);
        }

        [HttpGet]
        public IActionResult Crear(int id = 0)
        {
            var dto = new InquilinoRequestDTO(id, "", "", "", "", "", "");
            return View("formCrearModificar", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(InquilinoRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("formCrearModificar", dto);
            }
            service.AltaInquilino(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Modificar(int IdInquilino)
        {
            var inquilino = service.ObtenerPorId(IdInquilino);

            var dto = new InquilinoRequestDTO(
                IdInquilino,
                inquilino.Nombre!,
                inquilino.Apellido!,
                inquilino.Documento!,
                inquilino.Telefono!,
                inquilino.Email!,
                inquilino.Direccion!
            );

            return View("formCrearModificar", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modificar(InquilinoRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("formCrearModificar", dto);
            }
            service.ModificarInquilino(dto.IdInquilino, dto);
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            service.BajaInquilino(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult BuscarPorDocumento(string documento)
        {
            try
            {
                var inquilino = service.BuscarPorDocumento(documento);
                return Json(new { success = true, data = inquilino });
            }
            catch (NotFoundException)
            {
                return Ok(new { success = false, message = "No se encontró inquilino con ese documento." });
            }
        }
        
        [HttpGet]
        public IActionResult Detalles(int IdInquilino)
        {
            var dto = service.ObtenerPorId(IdInquilino);

            return View(dto);
        } 
    }
}