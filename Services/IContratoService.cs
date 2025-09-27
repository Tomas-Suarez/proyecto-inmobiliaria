using proyecto_inmobiliaria.Dtos.request;
using proyecto_inmobiliaria.Dtos.response;

namespace proyecto_inmobiliaria.Services
{
    public interface IContratoService
    {
        ContratoResponseDTO AltaContrato(ContratoRequestDTO dto);
        ContratoResponseDTO ModificarContrato(int ContratoId, ContratoRequestDTO dto);
        void BajaContrato(int ContratoId);
        IList<ContratoResponseDTO> TodosLosContratosPaginados(int paginaNro, int tamPagina);
        ContratoResponseDTO ObtenerPorId(int ContratoId);
        int CantidadTotalContrato();
        ContratoRequestDTO ObtenerRequestPorId(int ContratoId);
        IList<ContratoResponseDTO> ContratosPorInmueble(int idInmueble, int paginaNro, int tamPagina);
        int CantidadTotalPorInmueble(int idInmueble);
        PagoRequestDTO FinalizarContratoAnticipado(int idContrato);
        public void MarcarContratoComoFinalizado(int idContrato, bool anticipado);
    }
}