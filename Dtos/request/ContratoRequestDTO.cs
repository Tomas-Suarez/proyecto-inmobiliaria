using System.ComponentModel.DataAnnotations;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record ContratoRequestDTO(
        int IdContrato,

        [Required(ErrorMessage = "El inquilino es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un inquilino válido.")]
        int IdInquilino,

        [Required(ErrorMessage = "El inmueble es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un inmueble válido.")]
        int IdInmueble,

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, 99999999.99, ErrorMessage = "El monto está fuera del rango permitido.")]
        decimal Monto,

        DateTime FechaDesde,

        [Required(ErrorMessage = "La fecha de finalización es obligatoria")]
        DateTime FechaHasta,
        
        DateTime? FechaFinAnticipada,

        bool Finalizado
    );
}