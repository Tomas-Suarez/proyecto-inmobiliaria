using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IContratoService
    {
        ContratoResponseDTO AltaContrato(ContratoRequestDTO dto, int idUsuario);
        ContratoResponseDTO ModificarContrato(int ContratoId, ContratoRequestDTO dto, int idUsuario, bool auditar);
        void BajaContrato(int ContratoId);
        IList<ContratoResponseDTO> TodosLosContratosPaginados(int paginaNro, int tamPagina);
        ContratoResponseDTO ObtenerPorId(int ContratoId);
        int CantidadTotalContrato();
        ContratoRequestDTO ObtenerRequestPorId(int ContratoId);
        IList<ContratoResponseDTO> ContratosPorInmueble(int idInmueble, int paginaNro, int tamPagina);
        int CantidadTotalPorInmueble(int idInmueble);
        PagoRequestDTO FinalizarContratoAnticipado(int idContrato);
        void MarcarContratoComoFinalizado(int idContrato, int idUsuario, bool anticipado);
        int CantidadContratosVigentesPorFecha(DateTime fechaDesde, DateTime fechaHasta);
        IList<ContratoResponseDTO> ObtenerContratosVigentesPorFecha(DateTime fechaDesde, DateTime fechaHasta, int paginaNro, int tamPagina);
        public bool ExisteSuperposicion(int idInmueble, DateTime fechaDesde, DateTime fechaHasta, int? idContratoExcluir = null);
    }
}