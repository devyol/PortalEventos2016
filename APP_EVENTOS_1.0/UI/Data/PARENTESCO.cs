//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UI.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PARENTESCO
    {
        public PARENTESCO()
        {
            this.PARTICIPANTE_CONTACTO = new HashSet<PARTICIPANTE_CONTACTO>();
        }
    
        public decimal ID_PARENTESCO { get; set; }
        public string DESCRIPCION { get; set; }
        public string ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
    
        public virtual ICollection<PARTICIPANTE_CONTACTO> PARTICIPANTE_CONTACTO { get; set; }
    }
}
