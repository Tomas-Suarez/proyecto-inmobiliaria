using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IUsuarioRepository
    {
        Usuario Alta(Usuario usuario);
        int Baja(int idUsuario);
        Usuario Modificar(Usuario usuario);
        Usuario ObtenerPorId(int idUsuario);
        IList<Usuario> ObtenerLista(int paginaNro, int tamPagina);
        public int CantidadTotal();
        Usuario? ObtenerPorNombreUsuarioOEmail(string nombreUsuarioOEmail);

    }
}