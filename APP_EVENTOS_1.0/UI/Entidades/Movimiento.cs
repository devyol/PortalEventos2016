using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class Movimiento
    {
        public decimal id_movimiento { get; set; }
        public decimal id_participante { get; set; }
        public decimal id_evento { get; set; }
        public string tipo_mov { get; set; }
        public decimal tipo_pago { get; set; }
        public string tipo_pago_d { get; set; }
        public string descripcion { get; set; }
        public decimal? valor { get; set; }
        public decimal monto_total_evento { get; set; }
        public decimal monto_abonado { get; set; }
        public decimal saldo_pendiente { get; set; }
        public string fecha_movimiento { get; set; }        
        public string usuario { get; set; }

    }
}