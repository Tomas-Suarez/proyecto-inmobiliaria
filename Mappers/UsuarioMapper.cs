using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Mappers
{
    public class UsuarioMapper
    {
        public Usuario ToEntity(UsuarioRequestDTO dto)
        {
            return new Usuario
            {
                IdUsuario = dto.IdUsuario,
                NombreUsuario = dto.NombreUsuario,
                Email = dto.Email,
                Contrasena = dto.Contrasena,
                Rol = dto.Rol
            };
        }

        public UsuarioResponseDTO ToDto(Usuario entity)
        {
            return new UsuarioResponseDTO(
                entity.IdUsuario,
                entity.NombreUsuario!,
                entity.Email!,
                entity.Rol
            );
        }
    }
}