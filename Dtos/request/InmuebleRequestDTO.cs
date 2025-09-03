using System.ComponentModel.DataAnnotations;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record InmuebleRequestDTO(
        int IdInmueble,

        [Required(ErrorMessage = "Debe seleccionar un tipo de inmueble.")]
        int IdTipoInmueble,

        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        int IdEstadoInmueble,

        [Required(ErrorMessage = "Debe seleccionar un propietario.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un propietario válido.")]
        int IdPropietario,

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar 200 caracteres.")]
        string Direccion,

        [Required(ErrorMessage = "La cantidad de ambientes es obligatoria.")]
        [Range(1, 50, ErrorMessage = "La cantidad de ambientes debe estar entre 1 y 50.")]
        int CantidadAmbientes,
        
        [Required(ErrorMessage = "La superficie es obligatoria.")]
        [Range(1, 10000, ErrorMessage = "La superficie debe ser mayor a 0 y menor a 10,000 m².")]
        decimal SuperficieM2
    );
}