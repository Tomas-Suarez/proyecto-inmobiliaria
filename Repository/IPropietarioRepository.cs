using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IPropietarioRepository
    {
        int Alta(Propietario propietario);
        int Modificar(Propietario propietario);
        int Baja(int idPropietario);
    }
}