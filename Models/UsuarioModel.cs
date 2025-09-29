using proyecto_inmobiliaria.Enum;

namespace proyecto_inmobiliaria.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public ERol Rol { get; set; }
        public string? Email { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contrasena { get; set; }
    }
}