namespace Exceptions
{
    public class NotFoundException : Exception
    {
        public int Status = 404; 
        public NotFoundException(string mensaje) : base(mensaje) { }
    }
}