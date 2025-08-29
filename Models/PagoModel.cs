namespace proyecto_inmobiliaria.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public Boolean pagado { get; set; }
        public string? metodo_pago { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }

    }
}