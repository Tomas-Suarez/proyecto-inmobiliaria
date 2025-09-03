using System.ComponentModel.DataAnnotations;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record ContratoRequestDTO(
        int IdContrato,

        [Required(ErrorMessage = "El inquilino es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un inquilino válido.")]
        int IdInquilino,

        [Required(ErrorMessage = "El inmueble es obligatorio")]
        int IdInmueble,

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        decimal Monto,

        DateTime? FechaDesde,

        [Required(ErrorMessage = "La fecha de finalización es obligatoria")]
        DateTime? FechaHasta
    );
}