using proyecto_inmobiliaria.Enum;

namespace proyecto_inmobiliaria.Models
{
    public class AuditoriaPago
    {
        public int IdAuditoriaPago { get; set; }
        public int IdPago { get; set; }
        public int IdUsuario { get; set; }
        public Auditoria Accion { get; set; }
        public DateTime FechaAccion;
    }
}