using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IAuditoriaContratoRepository
    {
        AuditoriaContrato Registrar(AuditoriaContrato auditoria);
        IList<AuditoriaContratoResponseDTO> ListarAuditoria(int paginaNro, int tamPagina);
        int CantidadAuditoria();
        AuditoriaContratoResponseDTO ObtenerPorId(int idAuditoriaContrato);
    }
}