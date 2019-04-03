using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class BusInscripcion
    {
        public decimal id_bus { get; set; }
        public decimal id_bus_new { get; set; }
        public decimal id_participante { get; set; }
        public decimal id_evento { get; set; }
        public decimal no_bus { get; set; }
        public decimal no_bus_new { get; set; }
        public string descripcion { get; set; }
        public string hora_salida { get; set; }
        public string estado_registro { get; set; }
        public string usuario { get; set; }
    }
}