namespace proyecto_inmobiliaria.Dtos.request
{
    public record PropietarioRequestDTO(
        int IdPropietario,
        string Nombre,
        string Apellido,
        string Documento,
        string Telefono,
        string Email,
        string Direccion
    );
}