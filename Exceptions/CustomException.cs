namespace proyecto_inmobiliaria.Exceptions
{
    public abstract class CustomException : Exception
    {
        public int Status { get; }

        public CustomException(string message, int status) : base(message)
        {
            Status = status;
        }
    }
}