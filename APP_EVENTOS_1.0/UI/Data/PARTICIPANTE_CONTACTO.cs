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
    
    public partial class PARTICIPANTE_CONTACTO
    {
        public decimal ID_CONTACTO { get; set; }
        public Nullable<decimal> ID_PARTICIPANTE { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDO { get; set; }
        public string TELEFONO { get; set; }
        public string MOVIL { get; set; }
        public string CORREO { get; set; }
        public Nullable<decimal> ID_PARENTESCO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
    
        public virtual PARENTESCO PARENTESCO { get; set; }
        public virtual PARTICIPANTE PARTICIPANTE { get; set; }
    }
}
