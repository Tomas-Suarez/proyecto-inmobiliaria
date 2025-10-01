using proyecto_inmobiliaria.Enum;

namespace proyecto_inmobiliaria.Dtos.response
{
public record AuditoriaPagoResponseDTO(
    int IdAuditoriaPago,
    int IdPago,
    int NumeroPago,
    int IdContrato,
    string NombreUsuario,
    string Email,
    Auditoria Accion,
    DateTime FechaAccion
);
}