using proyecto_inmobiliaria.Models;

namespace proyecto_inmobiliaria.Repository
{
    public interface IInquilinoRepository
    {
        int Alta(Inquilino inquilino);
        int Modificar(Inquilino inquilino);
        int Baja(int idInquilino);
    }
}