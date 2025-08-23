using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Mappers
{
    public class InquilinoMapper
    {

        public Inquilino ToEntity(InquilinoRequestDTO dto)
        {
            return new Inquilino
            {
                IdInquilino = dto.IdInquilino,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Documento = dto.Documento,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Direccion = dto.Direccion
            };
        }

        public InquilinoResponseDTO ToDto(Inquilino entity)
        {
            return new InquilinoResponseDTO(
                entity.IdInquilino,
                entity.Nombre,
                entity.Apellido,
                entity.Documento,
                entity.Telefono,
                entity.Email,
                entity.Direccion
            );
        }
    }
}