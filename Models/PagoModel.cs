namespace proyecto_inmobiliaria.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdContrato { get; set; }
        public string? MetodoPago { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string? Detalle { get; set; }
        public bool Anulado { get; set; }
    }
}