namespace proyecto_inmobiliaria.Dtos.response
{
    public record PagoResponseDTO(
        int? IdPago,
        int? IdContrato,
        string? MetodoPago,
        DateTime? FechaPago,
        decimal? Monto,
        string? Detalle,
        bool? Anulado
    );
}