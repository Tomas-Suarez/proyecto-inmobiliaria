using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Mappers
{
    public class AuditoriaContratoMapper
    {

        public AuditoriaContrato ToEntity(AuditoriaContratoRequestDTO dto)
        {
            return new AuditoriaContrato
            {
                IdAuditoriaContrato = dto.IdAuditoriaContrato,
                IdContrato = dto.IdContrato,
                IdUsuario = dto.IdUsuario,
                Accion = dto.Accion,
                FechaAccion = dto.FechaAccion
            };
        }
    }
}