using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UI.Entidades;
using UI.Data;

namespace UI.Models
{
    public class GestionEvento
    {
        public Response<List<Evento>> ListEventos()
        {
            Response<List<Evento>> result = new Response<List<Evento>>();
            result.code = -1;
            result.message = "Ocurrio un Error en BD al tratar de realizar la consulta";
            result.data = new List<Evento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder sqlConsulta = new StringBuilder();
                    sqlConsulta.Append(" select * ");
                    sqlConsulta.Append(" from evento ");
                    sqlConsulta.Append(" order by id_evento desc ");

                    var list = db.Database.SqlQuery<Evento>(sqlConsulta.ToString()).ToList<Evento>();
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
                result.message = "Ocurrio una excepcion al obtener el listado de Eventos";
                result.messageError = ex.ToString();
                return result;
            }            
        }

        public Response<Evento> ListEvento(Evento obj)
        {
            Response<Evento> result = new Response<Evento>();
            result.code = -1;
            result.message = "Ocurrio un Error en BD al tratar de obtener los datos";
            result.data = new Evento();
            result.totalRecords = 0;

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strSelect = new StringBuilder();
                    strSelect.Append(" select * from evento ");
                    strSelect.Append(" where id_evento = :id ");

                    var list = db.Database.SqlQuery<Evento>(strSelect.ToString(), new object[] { obj.id_evento })
                                                                                  .FirstOrDefault<Evento>();
                    if (list == null)
                    {
                        result.code = -1;
                        result.message = "No existe Evento Registrado";
                        result.data = new Evento();
                        result.totalRecords = 0;
                        return result;
                    }
                    result.data = list;
                }
                result.code = 0;
                result.message = "OK";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al momento de obtener los datos";
                result.message = ex.ToString();
                result.totalRecords = 0;
                return result;
            }
        }

        public Response<Evento> InsertEvento(Evento obj)
        {
            Response<Evento> result = new Response<Evento>();            
            result.code = -1;
            result.message = "Ocurrio un Error en BD al tratar de realizar una transacción";
            result.data = new Evento();
            result.totalRecords = 0;
            
            if (obj.nombre_evento == null || obj.nombre_evento == "" || 
                obj.fecha_inicio == null || obj.fecha_inicio == "" ||
                obj.fecha_fin == null || obj.fecha_fin == ""
                || obj.usuario==null || obj.usuario=="")
            {
                result.code = -1;
                result.data = new Evento();
                result.message = "Favor de verificar los campos requeridos, no es valido guardar datos vacios";
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {                    
                    StringBuilder sqlInsert = new StringBuilder();
                    sqlInsert.Append(" insert into evento  ");
                    sqlInsert.Append(" (id_evento, ");
                    sqlInsert.Append(" nombre_evento, ");
                    sqlInsert.Append(" fecha_inicio, ");
                    sqlInsert.Append(" fecha_fin, ");
                    sqlInsert.Append(" estado_registro, ");
                    sqlInsert.Append(" usuario_creacion, ");
                    sqlInsert.Append(" fecha_creacion) ");
                    sqlInsert.Append(" values( ");
                    sqlInsert.Append(" (select correlativo_disponible from correlativo where id_correlativo = 2 and estado_registro = 'A'), ");
                    sqlInsert.Append(" upper(:nombre_evento), ");
                    sqlInsert.Append(" :fecha_inicio, ");
                    sqlInsert.Append(" :fecha_fin, ");
                    sqlInsert.Append(" 'A', ");
                    sqlInsert.Append(" upper(:usuario_creacion), ");
                    sqlInsert.Append(" sysdate) ");

                    var resp = db.Database.ExecuteSqlCommand(sqlInsert.ToString(), new object[] { obj.nombre_evento,
                                                                                                  obj.fecha_inicio,
                                                                                                  obj.fecha_fin,
                                                                                                  obj.usuario});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se ha registrado el Evento de forma exitosa";
                return result;

            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al intentar registrar un Evento" + ex.ToString();
                result.messageError = ex.ToString();
                return result;
            }            
        }

        public Response<Evento> UpdateEvento(Evento obj)
        {
            Response<Evento> result = new Response<Evento>();
            result.code = -1;
            result.message = "Ocurrio un Error en BD al tratar de realizar una transacción";
            result.data = new Evento();

            if (obj.nombre_evento == null || obj.nombre_evento == "" ||
                obj.fecha_inicio == null || obj.fecha_inicio == "" ||
                obj.fecha_fin == null || obj.fecha_fin == "" ||
                obj.estado_registro==null || obj.estado_registro=="" ||
                obj.usuario==null || obj.usuario=="")
            {
                result.code = -1;
                result.data = new Evento();
                result.message = "Favor de verificar los campos requeridos, no es permitido Actualizar estos datos vacios";
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strUpdate = new StringBuilder();                    
                    strUpdate.Append(" UPDATE evento ");
                    strUpdate.Append(" SET nombre_evento     = upper(:nombre), ");
                    strUpdate.Append("   fecha_inicio        =:fechaini, ");
                    strUpdate.Append("   fecha_fin           =:fechafin, ");
                    strUpdate.Append("   estado_registro     =upper(:estado), ");
                    strUpdate.Append("   usuario_modificacion=upper(:usuario), ");
                    strUpdate.Append("   fecha_modificacion  =sysdate ");
                    strUpdate.Append(" WHERE id_evento       = :id ");

                    var resp = db.Database.ExecuteSqlCommand(strUpdate.ToString(), new object[] { obj.nombre_evento,
                                                                                                  obj.fecha_inicio,
                                                                                                  obj.fecha_fin,
                                                                                                  obj.estado_registro,
                                                                                                  obj.usuario,
                                                                                                  obj.id_evento});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se ha Actualizado el Registro de Evento de forma exitosa";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una Excepcion al intentar actualizar el Evento";
                result.messageError = ex.ToString();
                return result;
            }
        }
    }
}