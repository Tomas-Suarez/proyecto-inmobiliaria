using System.ComponentModel.DataAnnotations;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record InmuebleRequestDTO(
        int IdInmueble,
        int IdTipoInmueble,
        int IdPropietario,
        [Required] string Direccion,
        [Required] int CantidadAmbientes,
        [Required] decimal SuperficieM2
    );
}