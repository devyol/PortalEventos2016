using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class Inscripcion
    {
        public decimal id_evento { get; set; }
        public decimal id_participante { get; set; }
        public string nombre_evento { get; set; }
        public string fecha_inscripcion { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
        public string operacion { get; set; }
    }
}