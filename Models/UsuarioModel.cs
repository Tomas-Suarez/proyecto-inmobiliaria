namespace proyecto_inmobiliaria.Models
{
    public class Usuario : Persona
    {
        public int IdUsuario { get; set; }
        public Rol? Rol { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contrasena { get; set; }
    }
}