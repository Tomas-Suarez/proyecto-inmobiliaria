using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IPagoService
    {
        PagoResponseDTO AltaPago(PagoRequestDTO dto, int idUsuario);
        PagoResponseDTO ModificarPago(int PagoId, PagoRequestDTO dto, int idUsuario);
        void BajaPago(int PagoId, int idUsuario);
        IList<PagoResponseDTO> ObtenerPagosPaginados(int idContrato, int paginaNro, int tamPagina);
        PagoResponseDTO ObtenerPorId(int PagoId);
        int CantidadTotalPagos(int idContrato);
        int CantidadPagosRealizados(int idContrato);
    }
}