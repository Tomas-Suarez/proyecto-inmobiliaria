using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IPagoRepository
    {
        Pago Alta(Pago pago);
        Pago Modificar(Pago pago);
        int Baja(int idPago);
        Pago ObtenerPorId(int idPago);
        IList<Pago> ObtenerLista(int paginaNro, int tamPagina);
    }
}