using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Models;
using proyecto_inmobiliaria.Repository;
using static proyecto_inmobiliaria.Constants.InquilinoConstants;


namespace proyecto_inmobiliaria.Services
{
    public class InquilinoService : IInquilinoService
    {

        private readonly IInquilinoRepository _repository;
        private readonly InquilinoMapper _mapper;

        public InquilinoService(IInquilinoRepository repository, InquilinoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public InquilinoResponseDTO AltaInquilino(InquilinoRequestDTO dto)
        {
            var inquilino = _mapper.ToEntity(dto);
            inquilino = _repository.Alta(inquilino);
            return _mapper.ToDto(inquilino);
        }

        public void BajaInquilino(int InquilinoId)
        {
            _ = _repository.ObtenerPorId(InquilinoId)
                                ?? throw new NotFoundException(NO_SE_ENCONTRO_INQUILINO_POR_ID + InquilinoId);

            int filasAfectadas = _repository.Baja(InquilinoId);

            if (filasAfectadas == 0)
            {
                throw new DeleteFailedException(ERROR_AL_BORRAR_INQUILINO);
            }

        }

        public InquilinoResponseDTO ModificarInquilino(int InquilinoId, InquilinoRequestDTO dto)
        {
            _ = _repository.ObtenerPorId(InquilinoId)
                                ?? throw new NotFoundException(NO_SE_ENCONTRO_INQUILINO_POR_ID + InquilinoId);

            var inquilino = _mapper.ToEntity(dto);

            inquilino.IdInquilino = InquilinoId;

            inquilino = _repository.Modificar(inquilino);

            return _mapper.ToDto(inquilino);
        }

        public IList<InquilinoResponseDTO> TodosLosInquilinos()
        {
            var inquilinos = _repository.ObtenerTodos() ?? new List<Inquilino>();

            return inquilinos
                .Select(p => _mapper.ToDto(p))
                .ToList();
        }

        public InquilinoResponseDTO ObtenerPorId(int inquilinoId)
        {
            var inquilino = _repository.ObtenerPorId(inquilinoId)
                                ?? throw new NotFoundException(NO_SE_ENCONTRO_INQUILINO_POR_ID + inquilinoId);

            return _mapper.ToDto(inquilino);
        }

        public InquilinoResponseDTO BuscarPorDocumento(string documento)
        {
            var inquilino = _repository.BuscarPorDocumento(documento)
                                ?? throw new NotFoundException(NO_SE_ENCONTRO_INQUILINO_POR_DOCUMENTO);

            return _mapper.ToDto(inquilino);
        }
    }
}