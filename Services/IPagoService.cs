using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IPagoService
    {
        PagoResponseDTO AltaPago(PagoRequestDTO dto);
        PagoResponseDTO ModificarPago(int PagoId, PagoRequestDTO dto);
        void BajaPago(int PagoId);
        IList<PagoResponseDTO> ObtenerPagosPaginados(int idContrato, int paginaNro, int tamPagina);
        PagoResponseDTO ObtenerPorId(int PagoId);
        int CantidadTotalPagos(int idContrato);
    }
}