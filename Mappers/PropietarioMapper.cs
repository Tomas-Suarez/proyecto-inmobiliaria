using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Mappers
{
    public class PropietarioMapper
    {

        public Propietario ToEntity(PropietarioRequestDTO dto)
        {
            return new Propietario
            {
                IdPropietario = dto.IdPropietario,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Documento = dto.Documento,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Direccion = dto.Direccion
            };
        }

        public PropietarioResponseDTO ToDto(Propietario entity)
        {
            return new PropietarioResponseDTO(
                entity.IdPropietario,
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