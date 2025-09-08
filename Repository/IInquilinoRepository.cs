using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IInquilinoRepository
    {
        Inquilino Alta(Inquilino Inquilino);
        Inquilino Modificar(Inquilino Inquilino);
        int Baja(int idInquilino);
        Inquilino ObtenerPorId(int idInquilino);
        IList<Inquilino> ObtenerTodos();
        IList<Inquilino> ObtenerLista(int paginaNro, int tamPagina);
        Inquilino BuscarPorDocumento(string documento);
    }
}