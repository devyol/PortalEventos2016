using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class OpcionEvento
    {
        public decimal id { get; set; }
        public decimal evento { get; set; }
        public string descripcion { get; set; }
        public decimal? precio { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
        public string operacion { get; set; }
    }
}