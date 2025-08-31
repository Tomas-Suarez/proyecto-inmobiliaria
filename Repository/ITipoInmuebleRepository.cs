using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface ITipoInmuebleRepository
    {
        IList<TipoInmueble> ObtenerTipoInmueble();
    }
}