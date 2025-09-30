using proyecto_inmobiliaria.Enum;

namespace proyecto_inmobiliaria.Dtos.response
{
    public record AuditoriaContratoResponseDTO(
        int IdAuditoriaContrato,

        int IdContrato,

        string? NombreUsuario,

        string? Email,

        Auditoria Accion,

        DateTime FechaAccion
    );
}