using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Repository;
using proyecto_inmobiliaria.Repository.imp;

using static proyecto_inmobiliaria.Constants.ContratoConstants;

namespace proyecto_inmobiliaria.Services.imp
{
    public class ContratoService : IContratoService
    {

        private readonly IContratoRepository _contratoRepository;
        private readonly IPagoRepository _pagoRepository;

        private readonly ContratoMapper _mapper;

        public ContratoService(IContratoRepository contratoRepository, ContratoMapper mapper, IPagoRepository pagoRepository)
        {
            _contratoRepository = contratoRepository;
            _mapper = mapper;
            _pagoRepository = pagoRepository;
        }
        public ContratoResponseDTO AltaContrato(ContratoRequestDTO dto)
        {
            var contrato = _mapper.ToEntity(dto);
            contrato.Finalizado = false;
            contrato = _contratoRepository.Alta(contrato);

            return ObtenerPorId(contrato.IdContrato);
        }

        public void BajaContrato(int ContratoId)
        {
            _ = ObtenerPorId(ContratoId);

            int filasAfectadas = _contratoRepository.Baja(ContratoId);

            if (filasAfectadas == 0)
            {
                throw new DeleteFailedException(ERROR_AL_BORRAR_CONTRATO);
            }
        }

        public int CantidadTotalContrato()
        {
            return _contratoRepository.CantidadTotal();
        }

        public ContratoResponseDTO ModificarContrato(int ContratoId, ContratoRequestDTO dto)
        {
            _ = ObtenerPorId(ContratoId);

            var contrato = _mapper.ToEntity(dto);

            _ = _contratoRepository.Modificar(contrato);

            return _contratoRepository.ObtenerPorId(ContratoId);
        }

        public ContratoResponseDTO ObtenerPorId(int ContratoId)
        {
            var contrato = _contratoRepository.ObtenerPorId(ContratoId)
                                        ?? throw new NotFoundException(NO_SE_ENCONTRO_CONTRATO_POR_ID + ContratoId);
            return contrato;
        }

        public ContratoRequestDTO ObtenerRequestPorId(int ContratoId)
        {
            var contrato = _contratoRepository.ObtenerPorIdRequest(ContratoId)
                                        ?? throw new NotFoundException(NO_SE_ENCONTRO_CONTRATO_POR_ID + ContratoId);

            return new ContratoRequestDTO(
                contrato.IdContrato,
                contrato.IdInquilino,
                contrato.IdInmueble,
                contrato.Monto,
                contrato.FechaDesde,
                contrato.FechaHasta,
                contrato.FechaFinAnticipada,
                contrato.Finalizado
            );
        }

        public IList<ContratoResponseDTO> TodosLosContratosPaginados(int paginaNro, int tamPagina)
        {
            return _contratoRepository.ObtenerLista(paginaNro, tamPagina);
        }

        public void FinalizarContratoAnticipado(int idContrato)
        {
            var contrato = ObtenerPorId(idContrato);

            if (contrato.Finalizado)
            {
                throw new InvalidOperationException(ERROR_CONTRATO_FINALIZADO);
            }

            int totalMesesContrato = ((contrato.FechaHasta.Year - contrato.FechaDesde.Year) * 12) +
                         (contrato.FechaHasta.Month - contrato.FechaDesde.Month + 1);

            int pagosRealizados = _pagoRepository.CantidadPagosRealizados(idContrato);

            int mesesTranscurridos = ((DateTime.Today.Year - contrato.FechaDesde.Year) * 12) +
                             (DateTime.Today.Month - contrato.FechaDesde.Month + 1);

            if (pagosRealizados < mesesTranscurridos)
            {
                throw new InvalidOperationException(PAGOS_PENDIENTES);
            }
        }
    }
}