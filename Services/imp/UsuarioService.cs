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
        private readonly string _avatarsPath = "wwwroot/img";
        private readonly string _defaultAvatar = "/img/avatar-default.jpg";

        public UsuarioService(IUsuarioRepository usuarioRepository, UsuarioMapper usuarioMapper)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioMapper = usuarioMapper;
        }

        public UsuarioResponseDTO AltaUsuario(UsuarioRequestDTO dto)
        {
            var usuario = _usuarioMapper.ToEntity(dto);

            usuario.Contrasena = AuthHelper.HashPassword(dto.Contrasena);

            usuario.Avatar_url ??= _defaultAvatar;

            _usuarioRepository.Alta(usuario);

            return _usuarioMapper.ToDto(usuario);
        }

        public void BajaUsuario(int idUsuario)
        {
            var usuario = ObtenerPorId(idUsuario);

            //Borramos el avatar si no es el default
            if (!string.IsNullOrEmpty(usuario.AvatarUrl) && usuario.AvatarUrl != _defaultAvatar)
            {
                var pathViejo = Path.Combine(_avatarsPath, Path.GetFileName(usuario.AvatarUrl));
                if (File.Exists(pathViejo))
                    File.Delete(pathViejo);
            }

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

            var usuarioDto = _usuarioMapper.ToDto(usuario);

            var dtoConAvatar = usuarioDto with { AvatarUrl = usuarioDto.AvatarUrl ?? "/img/avatar-default.jpg" };

Console.WriteLine($"DEBUG: AvatarUrl del usuario {usuario.NombreUsuario} = {usuario.Avatar_url}");


            return dtoConAvatar;

        }


        public UsuarioResponseDTO ModificarUsuario(int idUsuario, UsuarioRequestDTO dto)
        {
            var usuarioExistente = _usuarioRepository.ObtenerPorId(idUsuario)
                                 ?? throw new NotFoundException(NO_SE_ENCONTRO_USUARIO_POR_ID + idUsuario);

            var usuario = _usuarioMapper.ToEntity(dto);
            usuario.IdUsuario = idUsuario;

            usuario.Avatar_url ??= usuarioExistente.Avatar_url;

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

        public UsuarioResponseDTO CambiarAvatar(int idUsuario, IFormFile nuevoArchivo)
        {
            var usuario = _usuarioRepository.ObtenerPorId(idUsuario)
                          ?? throw new NotFoundException(NO_SE_ENCONTRO_USUARIO_POR_ID + idUsuario);

            if (!string.IsNullOrEmpty(usuario.Avatar_url) && usuario.Avatar_url != _defaultAvatar)
            {
                var pathViejo = Path.Combine(_avatarsPath, Path.GetFileName(usuario.Avatar_url));
                if (File.Exists(pathViejo))
                    File.Delete(pathViejo);
            }

            var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(nuevoArchivo.FileName)}";
            var pathNuevo = Path.Combine(_avatarsPath, nombreArchivo);

            using (var stream = new FileStream(pathNuevo, FileMode.Create))
            {
                nuevoArchivo.CopyTo(stream);
            }

            usuario.Avatar_url = $"/img/{nombreArchivo}";

            _usuarioRepository.Modificar(usuario);

            return _usuarioMapper.ToDto(usuario);
        }

        public void EliminarAvatar(int idUsuario)
        {
            var usuario = _usuarioRepository.ObtenerPorId(idUsuario)
                            ?? throw new NotFoundException(NO_SE_ENCONTRO_USUARIO_POR_ID + idUsuario);

            if (!string.IsNullOrEmpty(usuario.Avatar_url) && usuario.Avatar_url != _defaultAvatar)
            {
                var pathViejo = Path.Combine(_avatarsPath, Path.GetFileName(usuario.Avatar_url));
                if (File.Exists(pathViejo))
                    File.Delete(pathViejo);
            }

            usuario.Avatar_url = _defaultAvatar;
            _usuarioRepository.Modificar(usuario);
        }
    }
}