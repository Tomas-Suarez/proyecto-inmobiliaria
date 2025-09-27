using Microsoft.AspNetCore.Http;

namespace proyecto_inmobiliaria.Exceptions
{
    public class PendingPaymentsException : CustomException
    {
        public int MesesPendientes { get; }
        public PendingPaymentsException(int mesesPendientes)
            : base($"El contrato tiene {mesesPendientes} mes(es) de alquiler pendientes.", 
                   StatusCodes.Status409Conflict)
        {
            MesesPendientes = mesesPendientes;
        }
    }
}
