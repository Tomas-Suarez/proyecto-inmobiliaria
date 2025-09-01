using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IInquilinoService
    {
        InquilinoResponseDTO AltaInquilino(InquilinoRequestDTO dto);
        InquilinoResponseDTO ModificarInquilino(int InquilinoId, InquilinoRequestDTO dto);
        void BajaInquilino(int InquilinoId);
        IList<InquilinoResponseDTO> TodosLosInquilinos();
        InquilinoResponseDTO ObtenerPorId(int InquilinoId);
        InquilinoResponseDTO BuscarPorDocumento(string documento);

    }
}