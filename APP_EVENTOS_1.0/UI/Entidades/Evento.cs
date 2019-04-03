using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class Evento
    {
        public decimal id_evento { get; set; }
        public string nombre_evento { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string estado_registro { get; set; }
        public string usuario_creacion { get; set; }
        public DateTime fecha_creacion { get; set; }
        public string usuario_modificacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public string usuario { get; set; }
        public string operacion { get; set; }
    }
}