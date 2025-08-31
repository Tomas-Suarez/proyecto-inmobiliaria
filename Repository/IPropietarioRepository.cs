using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IPropietarioRepository
    {
        Propietario Alta(Propietario propietario);
        Propietario Modificar(Propietario propietario);
        int Baja(int idPropietario);
        Propietario ObtenerPorId(int idPropietario);
        IList<Propietario> ObtenerTodos();
        IList<Propietario> ObtenerLista(int paginaNro, int tamPagina);
        Propietario BuscarPorDocumento(string documento);
        

    }
}