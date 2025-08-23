namespace proyecto_inmobiliaria.Dtos.response
{
    public record InquilinoResponseDTO(
        int? IdInquilino,
        string? Nombre,
        string? Apellido,
        string? Documento,
        string? Telefono,
        string? Email,
        string? Direccion
    );
}