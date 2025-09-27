using Microsoft.AspNetCore.Mvc;
using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Services;

namespace proyecto_inmobiliaria.Controllers
{
    public class PagoController : Controller
    {
        private readonly IPagoService _servicePago;
        private readonly IContratoService _serviceContrato;

        public PagoController(IPagoService servicePago, IContratoService serviceContrato)
        {
            _servicePago = servicePago;
            _serviceContrato = serviceContrato;
        }

        public IActionResult Index(int idContrato, int paginaNro = 1, int tamPagina = 10)
        {
            ViewData["ActivePage"] = "Pago";

            var pagos = _servicePago.ObtenerPagosPaginados(idContrato, paginaNro, tamPagina);
            int totalInmuebles = _servicePago.CantidadTotalPagos(idContrato);
            int totalPaginas = (int)Math.Ceiling((double)totalInmuebles / tamPagina);

            ViewData["IdContrato"] = idContrato;
            ViewData["PaginaActual"] = paginaNro;
            ViewData["TotalPaginas"] = totalPaginas;

            return View(pagos);
        }

        [HttpGet]
        public IActionResult Crear(int idContrato, bool esFinalizacion = false)
        {
            var numeroPago = _servicePago.CantidadTotalPagos(idContrato) + 1;
            var contrato = _serviceContrato.ObtenerPorId(idContrato);

            PagoRequestDTO dto;

            //En caso que sea finalizar anticipado llenamos el form de pago con los datos de la multa
            if (esFinalizacion)
            {
                dto = _serviceContrato.FinalizarContratoAnticipado(idContrato);
            }
            else
            {
                dto = new PagoRequestDTO(
                    0,
                    idContrato,
                    "",
                    DateTime.Now,
                    contrato.Monto,
                    "",
                    false,
                    numeroPago
                );
            }

            ViewData["EsFinalizacion"] = esFinalizacion;
            return View("formCrearModificar", dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(PagoRequestDTO dto, bool esFinalizacion = false)
        {
            if (!ModelState.IsValid)
            {
                ViewData["EsFinalizacion"] = esFinalizacion;
                return View("formCrearModificar", dto);
            }

            _servicePago.AltaPago(dto);

            // Si es un pago de finalización, marcamos contrato como finalizado
            if (esFinalizacion)
            {
                _serviceContrato.MarcarContratoComoFinalizado(dto.IdContrato);  //Nos marca la fecha de anticipacion y el estado de finalizado.
            }

            return RedirectToAction(nameof(Index), new { idContrato = dto.IdContrato });
        }


        [HttpGet]
        public IActionResult Modificar(int idPago)
        {
            var responseDto = _servicePago.ObtenerPorId(idPago);

            var requestDto = new PagoRequestDTO(
                responseDto.IdPago,
                responseDto.IdContrato,
                responseDto.MetodoPago ?? "",
                responseDto.FechaPago,
                responseDto.Monto,
                responseDto.Detalle ?? "",
                responseDto.Anulado ?? false,
                responseDto.NumeroPago
            );

            return View("formCrearModificar", requestDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modificar(PagoRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("formCrearModificar", dto);
            }
            _servicePago.ModificarPago(dto.IdPago, dto);
            return RedirectToAction(nameof(Index), new { idContrato = dto.IdContrato });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Anular(int idPago)
        {
            _servicePago.BajaPago(idPago);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Detalles(int IdPago)
        {
            var dto = _servicePago.ObtenerPorId(IdPago);

            return View(dto);
        }
    }
}