using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IAuditoriaPagoRepository
    {
        AuditoriaPago Registrar(AuditoriaPago auditoria);
        IList<AuditoriaPagoResponseDTO> ListarAuditoria(int paginaNro, int tamPagina);
        int CantidadAuditoria();
        AuditoriaPagoResponseDTO ObtenerPorId(int idAuditoriaPago);
    }
}