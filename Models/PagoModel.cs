namespace proyecto_inmobiliaria.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public bool Pagado { get; set; }
        public string? Metodo_pago { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }

    }
}