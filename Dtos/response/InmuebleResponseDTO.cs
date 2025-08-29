using System.ComponentModel.DataAnnotations;

namespace proyecto_inmobiliaria.Dtos.response
{
    public record InmuebleResponseDTO(
        int IdInmueble,
        string TipoInmueble,
        [Required] string NombreCompletoPropietario,
        [Required] string Direccion,
        [Required] int CantidadAmbientes,
        [Required] decimal SuperficieM2
    );
}