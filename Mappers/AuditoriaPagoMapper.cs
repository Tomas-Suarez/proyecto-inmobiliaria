using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Mappers
{
    public class AuditoriaPagoMapper
    {

        public AuditoriaPago ToEntity(AuditoriaPagoRequestDTO dto)
        {
            return new AuditoriaPago
            {
                IdAuditoriaPago = dto.IdAuditoriaPago,
                IdPago = dto.IdPago,
                IdUsuario = dto.IdUsuario,
                Accion = dto.Accion,
                FechaAccion = dto.FechaAccion
            };
        }
    }
}