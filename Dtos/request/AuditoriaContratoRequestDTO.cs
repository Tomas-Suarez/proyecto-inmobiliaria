using proyecto_inmobiliaria.Enum;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record AuditoriaContratoRequestDTO(

        int IdAuditoriaContrato,

        int IdContrato,

        int IdUsuario,

        Auditoria Accion,

        DateTime FechaAccion
    );
}