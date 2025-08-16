namespace proyecto_inmobiliaria.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public EstadoPago? EstadoPago { get; set; }
        public MetodoPago? MetodoPago { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }

    }
}