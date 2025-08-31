using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IPropietarioService
    {
        PropietarioResponseDTO AltaPropietario(PropietarioRequestDTO dto);
        PropietarioResponseDTO ModificarPropietario(int PropietarioId, PropietarioRequestDTO dto);
        void BajaPropietario(int PropietarioId);

        IList<PropietarioResponseDTO> TodosLosPropietarios();

        PropietarioResponseDTO ObtenerPorId(int PropietarioId);

        PropietarioResponseDTO BuscarPorDocumento(string documento);

    }
}