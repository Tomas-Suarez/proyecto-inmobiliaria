using System.ComponentModel.DataAnnotations;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record ContratoRequestDTO(
        int IdContrato,
        int IdInquilino,
        int IdInmueble,
        [Required] decimal Monto,
        [Required] DateTime? FechaDesde,
        [Required] DateTime? FechaHasta
    );
}