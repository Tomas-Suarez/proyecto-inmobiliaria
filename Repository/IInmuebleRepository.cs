using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IInmuebleRepository
    {
        Inmueble Alta(Inmueble inmueble);
        Inmueble Modificar(Inmueble inmueble);
        int Baja(int idInmueble);
        InmuebleResponseDTO ObtenerPorId(int idInmueble);
        Inmueble ObtenerPorIdRequest(int idInmueble);
        IList<InmuebleResponseDTO> ObtenerLista(int paginaNro, int tamPagina, int? idEstado);
        int CantidadTotal(int? estado);
        IList<InmuebleResponseDTO> ObtenerDireccionFiltro(string direccion);
        void ModificarEstadoInmueble(int idInmueble, int estadoInmueble);
        IList<InmuebleResponseDTO> ObtenerInmueblesPorPropietario(int idPropietario, int paginaNro, int tamPagina);
        int CantidadTotalPorPropietario(int idPropietario);

    }
}