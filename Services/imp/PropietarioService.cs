using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Models;
using proyecto_inmobiliaria.Repository;
using static proyecto_inmobiliaria.Constants.PropietarioConstants;


namespace proyecto_inmobiliaria.Services
{
    public class PropietarioService : IPropietarioService
    {

        private readonly IPropietarioRepository _repository;
        private readonly PropietarioMapper _mapper;

        public PropietarioService(IPropietarioRepository repository, PropietarioMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public PropietarioResponseDTO AltaPropietario(PropietarioRequestDTO dto)
        {
            var propietario = _mapper.ToEntity(dto);
            propietario = _repository.Alta(propietario);
            return _mapper.ToDto(propietario);
        }

        public void BajaPropietario(int PropietarioId)
        {
            _ = ObtenerPorId(PropietarioId);

            int filasAfectadas = _repository.Baja(PropietarioId);

            if (filasAfectadas == 0)
            {
                throw new DeleteFailedException(ERROR_AL_BORRAR_PROPIETARIO);
            }

        }

        public PropietarioResponseDTO ModificarPropietario(int PropietarioId, PropietarioRequestDTO dto)
        {
            _ = ObtenerPorId(PropietarioId);

            var propietario = _mapper.ToEntity(dto);

            propietario.IdPropietario = PropietarioId;

            propietario = _repository.Modificar(propietario);

            return _mapper.ToDto(propietario);
        }

        public IList<PropietarioResponseDTO> TodosLosPropietarios()
        {
            var propietarios = _repository.ObtenerTodos() ?? new List<Propietario>();

            return propietarios
                .Select(p => _mapper.ToDto(p))
                .ToList();
        }

        public PropietarioResponseDTO ObtenerPorId(int propietarioId)
        {
            var propietario = _repository.ObtenerPorId(propietarioId)
                                ?? throw new NotFoundException(NO_SE_ENCONTRO_PROPIETARIO_POR_ID + propietarioId);

            return _mapper.ToDto(propietario);
        }

        public PropietarioResponseDTO BuscarPorDocumento(string documento)
        {
            var propietario = _repository.BuscarPorDocumento(documento)
                                ?? throw new NotFoundException(NO_SE_ENCONTRO_PROPIETARIO_POR_DOCUMENTO);

            return _mapper.ToDto(propietario);
        }
    }
}