using System.ComponentModel.DataAnnotations;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record PagoRequestDTO(
        int IdPago,
        [Required(ErrorMessage = "El contrato es obligatorio.")]
        int IdContrato,

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [StringLength(50, ErrorMessage = "El método de pago no puede superar 50 caracteres.")]
        string MetodoPago,

        [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
        DateTime? FechaPago,

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        decimal? Monto,

        [StringLength(200, ErrorMessage = "El detalle no puede superar 200 caracteres.")]
        string? Detalle,

        bool Anulado,

        int NumeroPago
    );
}
