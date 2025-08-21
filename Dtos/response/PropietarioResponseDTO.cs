namespace proyecto_inmobiliaria.Dtos.response
{
    public record PropietarioResponseDTO(
        int? IdPropietario,
        string? Nombre,
        string? Apellido,
        string? Documento,
        string? Telefono,
        string? Email,
        string? Direccion
    );
}