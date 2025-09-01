using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Mappers
{
    public class ContratoMapper
    {

        public Contrato ToEntity(ContratoRequestDTO dto)
        {
            return new Contrato
            {
                IdContrato = dto.IdContrato,
                IdInquilino = dto.IdInquilino,
                IdInmueble = dto.IdInmueble,
                Monto = dto.Monto,
                FechaDesde = dto.FechaDesde,
                FechaHasta = dto.FechaHasta
            };
        }
    }
}