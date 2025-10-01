using proyecto_inmobiliaria.Enum;

namespace proyecto_inmobiliaria.Dtos.request
{
    public record AuditoriaPagoRequestDTO(

        int IdAuditoriaPago,

        int IdPago,

        int IdUsuario,

        Auditoria Accion,

        DateTime FechaAccion
    );
}