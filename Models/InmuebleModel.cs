namespace proyecto_inmobiliaria.Models
{
    public class Inmueble
    {
        public int IdInmueble { get; set; }
        public EstadoInmueble? EstadoInmueble { get; set; }
        public TipoInmueble? TipoInmueble { get; set; }
        public Propietario? Propietario { get; set; }
        public string? Direccion { get; set; }
        public int CantidadAmbientes { get; set; }
        public double SuperficieM2 { get; set; }
    }
}