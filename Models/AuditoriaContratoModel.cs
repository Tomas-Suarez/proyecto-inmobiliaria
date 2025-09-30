using proyecto_inmobiliaria.Enum;

namespace proyecto_inmobiliaria.Models
{
    public class AuditoriaContrato
    {
        public int IdAuditoriaContrato { get; set; }
        public int IdContrato { get; set; }
        public int IdUsuario { get; set; }
        public Auditoria Accion { get; set; }
        public DateTime FechaAccion;
    }
}