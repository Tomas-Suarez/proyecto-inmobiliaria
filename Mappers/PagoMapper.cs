using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Mappers
{
    public class PagoMapper
    {

        public Pago ToEntity(PagoRequestDTO dto)
        {
            return new Pago
            {
                IdPago = dto.IdPago,
                IdContrato = dto.IdContrato,
                MetodoPago = dto.MetodoPago,
                FechaPago = (DateTime)dto.FechaPago,
                Monto = (decimal)dto.Monto,
                Detalle = dto.Detalle,
                Anulado = dto.Anulado,
                NumeroPago = dto.NumeroPago
            };
        }

        public PagoResponseDTO ToDto(Pago entity)
        {
            return new PagoResponseDTO(
                entity.IdPago,
                entity.IdContrato,
                entity.MetodoPago,
                entity.FechaPago,
                entity.Monto,
                entity.Detalle,
                entity.Anulado,
                entity.NumeroPago
            );
        }
    }
}