using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class Bus
    {
        public decimal id_evento { get; set; }
        public decimal id_bus { get; set; }
        public decimal no_bus { get; set; }
        public string descripcion { get; set; }
        public decimal capacidad { get; set; }
        public decimal disponible { get; set; }
        public decimal ocupado { get; set; }
        public string hora_salida { get; set; }
    }
}