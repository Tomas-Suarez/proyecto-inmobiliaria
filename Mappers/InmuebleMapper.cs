using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Mappers
{
    public class InmuebleMapper
    {

        public Inmueble ToEntity(InmuebleRequestDTO dto)
        {
            return new Inmueble
            {
                IdInmueble = dto.IdInmueble,
                IdTipoInmueble = dto.IdTipoInmueble,
                IdEstadoInmueble = dto.IdEstadoInmueble,
                IdPropietario = dto.IdPropietario,
                Direccion = dto.Direccion,
                CantidadAmbientes = dto.CantidadAmbientes,
                SuperficieM2 = dto.SuperficieM2
            };
        }
    }
}