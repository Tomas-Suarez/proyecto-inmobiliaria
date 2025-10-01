using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IUsuarioService
    {
        UsuarioResponseDTO AltaUsuario(UsuarioRequestDTO dto);
        public UsuarioResponseDTO ModificarUsuario(int idUsuario, UsuarioRequestDTO dto);
        void BajaUsuario(int idUsuario);
        IList<UsuarioResponseDTO> TodosLosUsuariosPaginados(int paginaNro, int tamPagina);
        UsuarioResponseDTO ObtenerPorId(int idUsuario);
        int CantidadUsuario();
        void CambiarContrasena(int idUsuario, string nuevaContrasena);
        UsuarioResponseDTO Login(string nombreUsuarioOEmail, string contrasena);
        UsuarioResponseDTO CambiarAvatar(int idUsuario, IFormFile nuevoArchivo);

    }
}