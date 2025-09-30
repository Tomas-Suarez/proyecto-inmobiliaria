using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Repository;
using static proyecto_inmobiliaria.Constants.AuditoriaConstants;

namespace proyecto_inmobiliaria.Services.imp
{
    public class AuditoriaContratoService : IAuditoriaContratoService
    {
        private readonly IAuditoriaContratoRepository _auditoriaRepository;
        private readonly AuditoriaContratoMapper _auditoriaMapper;

        public AuditoriaContratoService(IAuditoriaContratoRepository auditoriaContratoRepository, AuditoriaContratoMapper auditoriaMapper)
        {
            _auditoriaMapper = auditoriaMapper;
            _auditoriaRepository = auditoriaContratoRepository;
        }

        public int CantidadAuditoria()
        {
            return _auditoriaRepository.CantidadAuditoria();
        }

        public IList<AuditoriaContratoResponseDTO> ListarAuditoria(int paginaNro, int tamPagina)
        {
            return _auditoriaRepository.ListarAuditoria(paginaNro, tamPagina);
        }

        public AuditoriaContratoResponseDTO ObtenerPorId(int IdAuditoriaContrato)
        {
            var auditoriaContrato = _auditoriaRepository.ObtenerPorId(IdAuditoriaContrato)
                                        ?? throw new NotFoundException(NO_SE_ENCONTRO_AUDITORIA_POR_ID + IdAuditoriaContrato);
            return auditoriaContrato;
        }

        public AuditoriaContratoResponseDTO Registrar(AuditoriaContratoRequestDTO dto)
        {
            var auditoriaContrato = _auditoriaMapper.ToEntity(dto);

            var registrada = _auditoriaRepository.Registrar(auditoriaContrato);

            return new AuditoriaContratoResponseDTO(
                registrada.IdAuditoriaContrato,
                registrada.IdContrato,
                "",
                "",
                registrada.Accion,
                registrada.FechaAccion
            );
        }
    }
}