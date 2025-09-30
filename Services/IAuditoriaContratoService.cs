using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IAuditoriaContratoService
    {
        IList<AuditoriaContratoResponseDTO> ListarAuditoria(int paginaNro, int tamPagina);
        AuditoriaContratoResponseDTO Registrar(AuditoriaContratoRequestDTO dto);
        int CantidadAuditoria();
        AuditoriaContratoResponseDTO ObtenerPorId(int IdAuditoriaContrato);
    }
}