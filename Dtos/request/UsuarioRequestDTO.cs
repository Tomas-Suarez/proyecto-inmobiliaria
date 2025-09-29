using System.ComponentModel.DataAnnotations;
using proyecto_inmobiliaria.Enum;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record UsuarioRequestDTO(
        int IdUsuario,

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(30, ErrorMessage = "El nombre no puede superar los 30 caracteres.")]
        string NombreUsuario,
        
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        string Contrasena,
        
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        string Email,

        ERol Rol
    );
}