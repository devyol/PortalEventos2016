using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Entidades
{
    public class Participante
    {
        public decimal id { get; set; }
        public decimal idevento { get; set; }
        public string nombrec { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string genero { get; set; }
        public string talla { get; set; }
        public string fecha_nacimiento { get; set; }
        public string alerjico { get; set; }
        public Alerjico alerjico_ { get; set; }
        public Genero genero_ { get; set; }
        public Estado estado_ { get; set; }
        public string observaciones { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
        public string operacion { get; set; }
        
    }
}