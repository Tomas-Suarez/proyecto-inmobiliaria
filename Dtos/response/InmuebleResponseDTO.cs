using System.ComponentModel.DataAnnotations;

namespace proyecto_inmobiliaria.Dtos.response
{
    public record InmuebleResponseDTO(
        int IdInmueble,
        string TipoInmueble,
        string EstadoInmueble,
        [Required] string NombreCompletoPropietario,
        [Required] string Direccion,
        [Required] int CantidadAmbientes,
        [Required] decimal SuperficieM2
    );
}