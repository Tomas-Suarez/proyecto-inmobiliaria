using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IAuditoriaPagoService
    {
        IList<AuditoriaPagoResponseDTO> ListarAuditoria(int paginaNro, int tamPagina);
        AuditoriaPagoResponseDTO Registrar(AuditoriaPagoRequestDTO dto);
        int CantidadAuditoria();
        AuditoriaPagoResponseDTO ObtenerPorId(int IdAuditoriaPago);
    }
}