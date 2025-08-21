namespace proyecto_inmobiliaria.Exceptions
{
    public class DeleteFailedException : Exception
    {
        public int Status = 409;
        public DeleteFailedException(string mensaje) : base(mensaje) { }
    }
}