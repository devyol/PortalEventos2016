using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class SaldoEvento
    {
        public decimal id_participante { get; set; }
        public decimal id_evento { get; set; }
        public string nombre_evento { get; set; }
        public string fecha_inscripcion { get; set; }
        public decimal valor { get; set; }
        public decimal monto_total_evento { get; set; }
        public decimal monto_abonado { get; set; }
        public decimal saldo_pendiente { get; set; }
    }
}