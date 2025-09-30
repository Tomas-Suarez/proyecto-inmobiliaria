using proyecto_inmobiliaria.Dtos.response;
using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IContratoRepository
    {
        Contrato Alta(Contrato contrato);
        Contrato Modificar(Contrato contrato);
        int Baja(int idContrato);
        ContratoResponseDTO ObtenerPorId(int idContrato);
        Contrato ObtenerPorIdRequest(int idContrato);
        IList<ContratoResponseDTO> ObtenerLista(int paginaNro, int tamPagina);
        int CantidadTotal();
        IList<ContratoResponseDTO> ObtenerContratosPorInmueble(int idInmueble, int paginaNro, int tamPagina);
        int CantidadTotalPorInmueble(int idInmueble);
        bool ExisteSuperposicion(int idInmueble, DateTime fechaDesde, DateTime fechaHasta, int? idContratoExcluir = null);
        IList<ContratoResponseDTO> ObtenerContratosVigentesPorFecha(DateTime fechaDesde, DateTime fechaHasta, int paginaNro, int tamPagina);
        int CantidadContratosVigentesPorFecha(DateTime fechaDesde, DateTime fechaHasta);
    }
}