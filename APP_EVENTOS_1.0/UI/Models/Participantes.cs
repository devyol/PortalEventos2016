using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UI.Entidades;
using UI.Data;

namespace UI.Models
{
    public class Participantes
    {

        #region Constantes

        private const string _insertParticipante = @"insert into participante
                                                        (id_participante,
                                                        nombre,
                                                        apellido,
                                                        --direccion,
                                                        telefono,
                                                        --correo,
                                                        genero,
                                                        talla, 
                                                        --fecha_nacimiento,
                                                        --alerjico,
                                                        --observaciones,
                                                        estado_registro,
                                                        usuario_creacion,
                                                        fecha_creacion)
                                                        values(
                                                        (select correlativo_disponible from correlativo where id_correlativo = 4 and estado_registro = 'A'), 
                                                        upper(:nombres), 
                                                        upper(:apellidos), 
                                                        --upper(:direccion), 
                                                        :telefono, 
                                                        --:correo, 
                                                        upper(:genero), 
                                                        upper(:talla), 
                                                        --:fecha_nacimiento, 
                                                        --upper(:alerjico), 
                                                        --:observaciones, 
                                                        'A', 
                                                        upper(:usuario), 
                                                        sysdate)";

        private const string _actualizarParticipante = @" update participante 
                                                         set nombre = upper(:nombres), 
                                                         apellido = upper(:apellidos), 
                                                         --direccion = upper(:direccion), 
                                                         telefono = :telefono, 
                                                         --correo = :correo, 
                                                         genero = upper(:genero), 
                                                         talla = upper(:talla), 
                                                         --fecha_nacimiento = :fecha_nacimiento, 
                                                         --alerjico = :alerjico, 
                                                         --observaciones = :observaciones, 
                                                         estado_registro = :estado, 
                                                         usuario_modificacion = upper(:usuario), 
                                                         fecha_modificacion = sysdate 
                                                         where id_participante = :id_participante";

        #endregion


        public Response<List<Participante>> ListaParticipantes()
        {
            Response<List<Participante>> result = new Response<List<Participante>>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al tratar de obtener la lista de Participantes";
            result.data = new List<Participante>();
            result.totalRecords = 0;

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strList = new StringBuilder();
                    strList.Append(" select id_participante id, ");
                    strList.Append(" nombre||' '||apellido nombrec ");
                    strList.Append(" from participante ");
                    strList.Append(" where estado_registro = 'A' ");
                    strList.Append(" order by nombre ");

                    var list = db.Database.SqlQuery<Participante>(strList.ToString()).ToList<Participante>();

                    foreach (var item in list)
                    {
                        result.data.Add(item);
                    }
                    result.totalRecords = list.Count();
                }
                result.code = 0;
                result.message = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al tratar de obtener la lista de Participantes";
                result.messageError = ex.ToString();
                return result;                
            }
        }

        public Response<Participante> ListarParticipante(decimal id)
        {
            Response<Participante> result = new Response<Participante>();
            result.code = 0;
            result.message = "Ocurrio un error de base de datos al obtener el registro del Participante";
            result.data = new Participante();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strList = new StringBuilder();
                    strList.Append(" select id_participante id, nombre||' '||apellido nombrec, nombre,apellido,direccion,telefono,correo,genero, talla, ");
                    strList.Append(" to_char(fecha_nacimiento,'dd/mm/yyyy') fecha_nacimiento,alerjico,observaciones,estado_registro estado ");
                    strList.Append(" from participante ");
                    strList.Append(" where id_participante = :id_participante ");

                    var list = db.Database.SqlQuery<Participante>(strList.ToString(), new object[] { id }).SingleOrDefault<Participante>();

                    if (list == null)
                    {
                        result.code = -1;
                        result.message = "No existe Participante registrado";
                        result.data = new Participante();
                        result.totalRecords = 0;
                        return result;                        
                    }

                    //if (list.alerjico == "S")
                    //{
                    //    list.alerjico_ = new Alerjico();
                    //    list.alerjico_.alerjico = "S";
                    //    list.alerjico_.descripcion = "SI";                        
                    //}

                    //if (list.alerjico == "N")
                    //{
                    //    list.alerjico_ = new Alerjico();
                    //    list.alerjico_.alerjico = "N";
                    //    list.alerjico_.descripcion = "NO";
                    //}

                    if (list.genero == "M")
                    {
                        list.genero_ = new Genero();
                        list.genero_.genero = "M";
                        list.genero_.descripcion = "MASCULINO";
                    }

                    if (list.genero == "F")
                    {
                        list.genero_ = new Genero();
                        list.genero_.genero = "F";
                        list.genero_.descripcion = "FEMENINO";
                    }

                    if (list.estado == "A")
                    {
                        list.estado_ = new Estado();
                        list.estado_.estado = "A";
                        list.estado_.descripcion = "ACTIVO";
                    }

                    if (list.estado == "B")
                    {
                        list.estado_ = new Estado();
                        list.estado_.estado = "B";
                        list.estado_.descripcion = "INACTIVO";
                    }

                    result.data = list;
                }
                result.code = 0;
                result.message = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al tratar de obtener el registro de Participante";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<Participante> RegistrarParticipante(Participante pa)
        {
            Response<Participante> result = new Response<Participante>();
            result.code = -1;
            result.message = "Ocurrio un Error en la base de datos al intentar registrar un Participante";
            result.data = new Participante();

            if (pa.nombre == null || pa.nombre == "" ||
                pa.apellido == null || pa.apellido == "" ||
                pa.usuario == null || pa.usuario == "")
            {
                result.code = -1;
                result.message = "Favor de verificar los datos de nombre y apellido que no sean datos vacios, no es permitido registrar datos vacios";
                result.data = new Participante();
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strIns = new StringBuilder();
                    strIns.Append(_insertParticipante);

                    var resp = db.Database.ExecuteSqlCommand(strIns.ToString(), new object[] { pa.nombre,
                                                                                               pa.apellido,
                                                                                               pa.telefono,
                                                                                               pa.genero_.genero,
                                                                                               pa.talla,
                                                                                               pa.usuario});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se ha registrado el Participante de forma exitosa";
                return result;

            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al intentar registrar el Participante";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<Participante> ActualizarParticipante(Participante pa)
        {
            Response<Participante> result = new Response<Participante>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al intentar actualizar el participante";
            result.data = new Participante();

            if (pa.nombre == null || pa.nombre == "" ||
                pa.apellido == null || pa.apellido == "" ||
                pa.usuario == null || pa.usuario == "")
            {
                result.code = -1;
                result.message = "Favor de verificar los datos, no es permitido registrar datos vacios";
                result.data = new Participante();
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strUpd = new StringBuilder();
                    strUpd.Append(_actualizarParticipante);

                    var resp = db.Database.ExecuteSqlCommand(strUpd.ToString(), new object[] { pa.nombre,
                                                                                               pa.apellido,                                                                                               
                                                                                               pa.telefono,
                                                                                               pa.genero_.genero,
                                                                                               pa.talla,
                                                                                               pa.estado_.estado,
                                                                                               pa.usuario,
                                                                                               pa.id});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se Actualizaron los datos del Participante de forma exitosa";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al tratar de actualizar los datos: " + ex.ToString();
                result.messageError = ex.ToString();
                return result;
            }
        }
    }
}