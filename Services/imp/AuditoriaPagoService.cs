using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Repository;
using static proyecto_inmobiliaria.Constants.AuditoriaConstants;

namespace proyecto_inmobiliaria.Services.imp
{
    public class AuditoriaPagoService : IAuditoriaPagoService
    {
        private readonly IAuditoriaPagoRepository _auditoriaRepository;
        private readonly AuditoriaPagoMapper _auditoriaMapper;

        public AuditoriaPagoService(IAuditoriaPagoRepository auditoriaPagoRepository, AuditoriaPagoMapper auditoriaMapper)
        {
            _auditoriaMapper = auditoriaMapper;
            _auditoriaRepository = auditoriaPagoRepository;
        }

        public int CantidadAuditoria()
        {
            return _auditoriaRepository.CantidadAuditoria();
        }

        public IList<AuditoriaPagoResponseDTO> ListarAuditoria(int paginaNro, int tamPagina)
        {
            return _auditoriaRepository.ListarAuditoria(paginaNro, tamPagina);
        }

        public AuditoriaPagoResponseDTO ObtenerPorId(int IdAuditoriaPago)
        {
            var auditoriaPago = _auditoriaRepository.ObtenerPorId(IdAuditoriaPago)
                                        ?? throw new NotFoundException(NO_SE_ENCONTRO_AUDITORIA_POR_ID + IdAuditoriaPago);
            return auditoriaPago;
        }

        public AuditoriaPagoResponseDTO Registrar(AuditoriaPagoRequestDTO dto)
        {
            var auditoriaPago = _auditoriaMapper.ToEntity(dto);

            var registrada = _auditoriaRepository.Registrar(auditoriaPago);

            return ObtenerPorId(registrada.IdAuditoriaPago);
        }
    }
}