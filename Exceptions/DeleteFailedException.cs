namespace proyecto_inmobiliaria.Exceptions
{
    public class DeleteFailedException : CustomException
    {
        public DeleteFailedException(string message) 
            : base(message, StatusCodes.Status409Conflict) 
        { 
        }
    }
}
