using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class InscripcionOpcion
    {
        public int id_participante { get; set; }
        public int id_evento { get; set; }
        public int id_opcion { get; set; }
        public string estado { get; set; }
        public bool estadoOpcion { get; set; }
        public string obligatorio { get; set; }
        public string es_transporte { get; set; }
        public Estado estador { get; set; }
        public string opcion { get; set; }
        public decimal precio { get; set; }
        public string operacion { get; set; }
        public string usuario { get; set; }
    }
}