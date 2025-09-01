namespace proyecto_inmobiliaria.Dtos.response
{
    public record ContratoResponseDTO(
        int IdContrato,
        string DireccionInmueble,
        string TipoInmueble,
        string NombreCompletoInquilino,
        decimal Monto,
        DateTime FechaDesde,
        DateTime FechaHasta,
        string EstadoContrato
    );
}