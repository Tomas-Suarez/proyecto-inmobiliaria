namespace proyecto_inmobiliaria.Models
{
    public class Contrato
    {
        public int IdContrato { get; set; }
        public Inquilino? Inquilino { get; set; }
        public Inmueble? Inmueble { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}