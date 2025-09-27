namespace proyecto_inmobiliaria.Exceptions
{
    public class ContractAlreadyFinalizedException  : CustomException
    {
        public ContractAlreadyFinalizedException (string message) 
            : base(message, StatusCodes.Status409Conflict) 
        { 
        }
    }
}
