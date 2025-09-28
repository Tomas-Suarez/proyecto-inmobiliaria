namespace proyecto_inmobiliaria.Exceptions
{
    public class ContractOverlapException : CustomException
    {
        public ContractOverlapException(string message) 
            : base(message, StatusCodes.Status409Conflict) 
        { 
        }
    }
}
