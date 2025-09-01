using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Repository.imp;

using static proyecto_inmobiliaria.Constants.ContratoConstants;

namespace proyecto_inmobiliaria.Services.imp
{
    public class ContratoService : IContratoService
    {

        private readonly IContratoRepository _repository;

        private readonly ContratoMapper _mapper;

        public ContratoService(IContratoRepository repository, ContratoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public ContratoResponseDTO AltaContrato(ContratoRequestDTO dto)
        {
            var contrato = _mapper.ToEntity(dto);
            contrato = _repository.Alta(contrato);
            return ObtenerPorId(contrato.IdContrato);
        }

        public void BajaContrato(int ContratoId)
        {
            _ = ObtenerPorId(ContratoId);

            int filasAfectadas = _repository.Baja(ContratoId);

            if (filasAfectadas == 0)
            {
                throw new DeleteFailedException(ERROR_AL_BORRAR_CONTRATO);
            }
        }

        public int CantidadTotalContrato()
        {
            return _repository.CantidadTotal();
        }

        public ContratoResponseDTO ModificarContrato(int ContratoId, ContratoRequestDTO dto)
        {
            _ = ObtenerPorId(ContratoId);

            var contrato = _mapper.ToEntity(dto);

            _ = _repository.Modificar(contrato);

            return _repository.ObtenerPorId(ContratoId);
        }

        public ContratoResponseDTO ObtenerPorId(int ContratoId)
        {
            var contrato = _repository.ObtenerPorId(ContratoId)
                                        ?? throw new NotFoundException(NO_SE_ENCONTRO_CONTRATO_POR_ID + ContratoId);
            return contrato;
        }

        public ContratoRequestDTO ObtenerRequestPorId(int ContratoId)
        {
            var contrato = _repository.ObtenerPorIdRequest(ContratoId)
                                        ?? throw new NotFoundException(NO_SE_ENCONTRO_CONTRATO_POR_ID + ContratoId);

            return new ContratoRequestDTO(
                contrato.IdContrato,
                contrato.IdInquilino,
                contrato.IdInmueble,
                contrato.Monto,
                contrato.FechaDesde,
                contrato.FechaHasta
            );
        }

        public IList<ContratoResponseDTO> TodosLosContratosPaginados(int paginaNro, int tamPagina)
        {
            return _repository.ObtenerLista(paginaNro, tamPagina);
        }
    }
}