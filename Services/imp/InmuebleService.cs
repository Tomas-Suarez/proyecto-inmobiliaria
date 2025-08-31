using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Repository;

using static proyecto_inmobiliaria.Constants.InmuebleConstants;

namespace proyecto_inmobiliaria.Services.imp
{
    public class InmuebleService : IInmuebleService
    {
        private readonly IInmuebleRepository _repository;
        private readonly InmuebleMapper _mapper;

        public InmuebleService(IInmuebleRepository repository, InmuebleMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public InmuebleResponseDTO AltaInmueble(InmuebleRequestDTO dto)
        {
            var inmueble = _mapper.ToEntity(dto);
            inmueble.IdEstadoInmueble = INMUEBLE_ESTADO_DISPONIBLE;
            inmueble = _repository.Alta(inmueble);
            return ObtenerPorId(inmueble.IdInmueble);
        }

        public void BajaInmueble(int InmuebleId)
        {
            _ = ObtenerPorId(InmuebleId);

            int filasAfectadas = _repository.Baja(InmuebleId);

            if (filasAfectadas == 0)
            {
                throw new DeleteFailedException(ERROR_AL_BORRAR_INMUEBLE);
            }
        }

        public int CantidadTotalInmuebles()
        {
            return _repository.CantidadTotal();
        }

        public InmuebleResponseDTO ModificarInmueble(int InmuebleId, InmuebleRequestDTO dto)
        {
            _ = ObtenerPorId(InmuebleId);

            var inmueble = _mapper.ToEntity(dto);

            _ = _repository.Modificar(inmueble);

            return _repository.ObtenerPorId(InmuebleId);
        }

        public InmuebleResponseDTO ObtenerPorId(int InmuebleId)
        {
            var inmueble = _repository.ObtenerPorId(InmuebleId)
                                ?? throw new NotFoundException(NO_SE_ENCONTRO_INMUEBLE_POR_ID + InmuebleId);
            return inmueble;
        }

        public IList<InmuebleResponseDTO> TodosLosInmueblesPaginados(int paginaNro, int tamPagina)
        {
            return _repository.ObtenerLista(paginaNro, tamPagina);
        }

        public InmuebleRequestDTO ObtenerRequestPorId(int inmuebleId)
        {
            var inmueble = _repository.ObtenerPorIdRequest(inmuebleId)
                              ?? throw new NotFoundException(NO_SE_ENCONTRO_INMUEBLE_POR_ID + inmuebleId);

            return new InmuebleRequestDTO(
                inmueble.IdInmueble,
                inmueble.IdTipoInmueble,
                inmueble.IdEstadoInmueble,
                inmueble.IdPropietario,
                inmueble.Direccion!,
                inmueble.CantidadAmbientes,
                inmueble.SuperficieM2
            );
        }

    }
}