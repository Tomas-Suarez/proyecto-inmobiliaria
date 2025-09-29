using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Exceptions;
using proyecto_inmobiliaria.Helper;
using proyecto_inmobiliaria.Mappers;
using proyecto_inmobiliaria.Repository;

using static proyecto_inmobiliaria.Constants.UsuarioConstants;

namespace proyecto_inmobiliaria.Services.imp
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UsuarioMapper _usuarioMapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, UsuarioMapper usuarioMapper)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioMapper = usuarioMapper;
        }

        public UsuarioResponseDTO AltaUsuario(UsuarioRequestDTO dto)
        {
            var usuario = _usuarioMapper.ToEntity(dto);

            usuario.Contrasena = AuthHelper.HashPassword(dto.Contrasena);

            _usuarioRepository.Alta(usuario);

            return _usuarioMapper.ToDto(usuario);
        }

        public void BajaUsuario(int idUsuario)
        {
            ObtenerPorId(idUsuario);

            _usuarioRepository.Baja(idUsuario);
        }

        public void CambiarContrasena(int idUsuario, string nuevaContrasena)
        {
            var usuario = _usuarioRepository.ObtenerPorId(idUsuario)
                            ?? throw new NotFoundException(NO_SE_ENCONTRO_USUARIO_POR_ID + idUsuario);

            usuario.Contrasena = AuthHelper.HashPassword(nuevaContrasena);
            _usuarioRepository.Modificar(usuario);

        }

        public int CantidadUsuario()
        {
            return _usuarioRepository.CantidadTotal();
        }

        public UsuarioResponseDTO Login(string nombreUsuarioOEmail, string contrasena)
        {
            var usuario = _usuarioRepository.ObtenerPorNombreUsuarioOEmail(nombreUsuarioOEmail);

            if (usuario == null || !AuthHelper.VerificarPassword(contrasena, usuario.Contrasena!))
            {
                throw new UnauthorizedAccessException(CREDENCIALES_INVALIDAS);
            }

            return _usuarioMapper.ToDto(usuario);
        }

        public UsuarioResponseDTO ModificarUsuario(int idUsuario, UsuarioRequestDTO dto)
        {
            ObtenerPorId(idUsuario);

            var usuario = _usuarioMapper.ToEntity(dto);

            usuario.IdUsuario = idUsuario;

            usuario = _usuarioRepository.Modificar(usuario);

            return _usuarioMapper.ToDto(usuario);
        }

        public UsuarioResponseDTO ObtenerPorId(int idUsuario)
        {
            var usuario = _usuarioRepository.ObtenerPorId(idUsuario)
                            ?? throw new NotFoundException(NO_SE_ENCONTRO_USUARIO_POR_ID + idUsuario);

            return _usuarioMapper.ToDto(usuario);
        }

        public IList<UsuarioResponseDTO> TodosLosUsuariosPaginados(int paginaNro, int tamPagina)
        {
            var usuario = _usuarioRepository.ObtenerLista(paginaNro, tamPagina);

            if (usuario == null || usuario.Count == 0)
            {
                return new List<UsuarioResponseDTO>();
            }

            return usuario.Select(p => _usuarioMapper.ToDto(p)).ToList();
        }
    }
}