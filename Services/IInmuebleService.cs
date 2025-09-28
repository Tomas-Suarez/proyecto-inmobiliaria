using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IInmuebleService
    {
        InmuebleResponseDTO AltaInmueble(InmuebleRequestDTO dto);
        InmuebleResponseDTO ModificarInmueble(int InmuebleId, InmuebleRequestDTO dto);
        void BajaInmueble(int InmuebleId);
        IList<InmuebleResponseDTO> TodosLosInmueblesPaginados(int paginaNro, int tamPagina, int? estado);
        InmuebleResponseDTO ObtenerPorId(int InmuebleId);
        int CantidadTotalInmuebles(int? estado);
        InmuebleRequestDTO ObtenerRequestPorId(int inmuebleId);
        IList<InmuebleResponseDTO> BuscarPorDireccion(string direccion);
        void CambiarEstado(int idInmueble, int nuevoEstado);
        IList<InmuebleResponseDTO> ObtenerInmueblesPorPropietario(int IdPropietario, int paginaNro, int tamPagina);
        int CantidadTotalPorPropietario(int idPropietario);
        int CantidadTotalDisponiblesPorFecha(DateTime fechaDesde, DateTime fechaHasta);
        IList<InmuebleResponseDTO> ObtenerDisponiblesPorFecha(DateTime fechaDesde, DateTime fechaHasta, int paginaNro, int tamPagina);
    }
}