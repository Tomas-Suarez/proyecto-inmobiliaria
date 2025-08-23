using System.ComponentModel.DataAnnotations;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record InquilinoRequestDTO(
        int IdInquilino,
        [Required] string Nombre,
        [Required] string Apellido,
        [Required] string Documento,
        [Required] string Telefono,
        [Required] string Email,
        [Required] string Direccion
    );
}