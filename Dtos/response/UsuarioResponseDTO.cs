using proyecto_inmobiliaria.Enum;

namespace proyecto_inmobiliaria.Dtos.response
{
    public record UsuarioResponseDTO(
        int IdUsuario,
        string NombreUsuario,
        string Email,
        ERol Rol
    );
}