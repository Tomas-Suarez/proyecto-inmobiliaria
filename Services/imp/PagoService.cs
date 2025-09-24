using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Repository;

using static proyecto_inmobiliaria.Constants.PagoConstants;

namespace proyecto_inmobiliaria.Services.imp
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _repository;
        private readonly PagoMapper _mapper;

        public PagoService(IPagoRepository repository, PagoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public PagoResponseDTO AltaPago(PagoRequestDTO dto)
        {
            var pago = _mapper.ToEntity(dto);
            pago = _repository.Alta(pago);
            return _mapper.ToDto(pago);
        }

        public void BajaPago(int PagoId)
        {
            _ = ObtenerPorId(PagoId);

            int filasAfectadas = _repository.Baja(PagoId);

            if (filasAfectadas == 0)
            {
                throw new DeleteFailedException(ERROR_AL_BORRAR_PAGO);
            }

        }

        public int CantidadTotalPagos(int idContrato)
        {
            return _repository.ContarPagos(idContrato);
        }

        public PagoResponseDTO ModificarPago(int PagoId, PagoRequestDTO dto)
        {
            _ = ObtenerPorId(PagoId);

            var pago = _mapper.ToEntity(dto);

            pago.IdPago = PagoId;

            pago = _repository.Modificar(pago);

            return _mapper.ToDto(pago);
        }

        public IList<PagoResponseDTO> ObtenerPagosPaginados(int idContrato, int paginaNro, int tamPagina)
        {
            var pagos = _repository.ObtenerLista(idContrato, paginaNro, tamPagina);

            if (pagos == null || pagos.Count == 0)
            {
                return new List<PagoResponseDTO>();
            }

            return pagos.Select(p => _mapper.ToDto(p)).ToList();
        }

        public PagoResponseDTO ObtenerPorId(int PagoId)
        {
            var pago = _repository.ObtenerPorId(PagoId)
                            ?? throw new NotFoundException(NO_SE_ENCONTRO_PAGO_POR_ID + PagoId);
            return _mapper.ToDto(pago);
        }
    }
}