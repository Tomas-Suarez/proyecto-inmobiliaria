using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IEstadoInmuebleRepository
    {
        IList<EstadoInmueble> ObtenerEstadoInmueble();
    }
}