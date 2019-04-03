using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UI.Entidades;
using UI.Data;

namespace UI.Models
{
    public class OpcionesEventos
    {
        public Response<List<Evento>> ListaEventosActivos()
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
                    sqlConsulta.Append(" select id_evento, nombre_evento ");
                    sqlConsulta.Append(" from evento ");
                    sqlConsulta.Append(" where estado_registro = 'A' ");
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

        public Response<List<OpcionEvento>> ListarOpcionesEvento(decimal id)
        {
            Response<List<OpcionEvento>> result = new Response<List<OpcionEvento>>();
            result.code = -1;
            result.message = "Ocurrio un Error en BD al tratar de obtener los datos";
            result.data = new List<OpcionEvento>();
            result.totalRecords = 0;

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strSel = new StringBuilder();
                    strSel.Append(" select id_opcion id, id_evento evento, descripcion, ");
                    strSel.Append(" precio, estado_registro estado");
                    strSel.Append(" from opcion_evento ");
                    strSel.Append(" where id_evento = :id_evento ");
                    strSel.Append(" order by id ");

                    var list = db.Database.SqlQuery<OpcionEvento>(strSel.ToString(), new object[] { id }).ToList<OpcionEvento>();
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
                result.message = "Ocurrio una Excepcion al tratar de obtener los datos";
                result.messageError = ex.ToString();
                return result;                
            }
        }

        public Response<OpcionEvento> ListarOpcionEvento(decimal id)
        {
            Response<OpcionEvento> result = new Response<OpcionEvento>();
            result.code = 1;
            result.message = "Ocurrio un error en la base de datos al tratar de obtener el registro";
            result.data = new OpcionEvento();
            result.totalRecords = 0;

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strList = new StringBuilder();
                    strList.Append(" select id_opcion id, id_evento evento, descripcion,  ");
                    strList.Append(" precio, estado_registro estado ");
                    strList.Append(" from opcion_evento ");
                    strList.Append(" where id_opcion = :id_opcion ");

                    var list = db.Database.SqlQuery<OpcionEvento>(strList.ToString(), new object[] { id }).SingleOrDefault<OpcionEvento>();

                    if (list == null)
                    {
                        result.code = -1;
                        result.message = "No existe Opcion del Evento registrado";
                        result.data = new OpcionEvento();
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

        public Response<OpcionEvento> InsertarOpcionEvento(OpcionEvento obj)
        {
            Response<OpcionEvento> result = new Response<OpcionEvento>();
            result.code = -1;
            result.message = "Ocurrio un error en Base de datos al intentar registrar Opcion";
            result.data = new OpcionEvento();
            result.totalRecords = 0;

            if (obj.evento.Equals(null) ||
                obj.descripcion==null || obj.descripcion == "" ||
                obj.precio.Equals(null)||
                obj.usuario==null || obj.usuario =="")
            {
                result.code = -1;
                result.data = new OpcionEvento();
                result.message = "Favor de Verificar los campos requeridos, no es valido guardar datos vacios";
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strInsert = new StringBuilder();                    
                    strInsert.Append(" insert into opcion_evento ");
                    strInsert.Append(" (id_opcion,id_evento,descripcion,precio,estado_registro,usuario_creacion,fecha_creacion) ");
                    strInsert.Append(" values( ");
                    strInsert.Append(" (select correlativo_disponible from correlativo where id_correlativo = 3 and estado_registro = 'A'), ");
                    strInsert.Append(" :id_evento, ");
                    strInsert.Append(" upper(:descripcion), ");
                    strInsert.Append(" :precio, ");
                    strInsert.Append(" 'A', ");
                    strInsert.Append(" upper(:usuario), ");
                    strInsert.Append(" sysdate) ");

                    var resp = db.Database.ExecuteSqlCommand(strInsert.ToString(), new object[] { obj.evento, 
                                                                                                  obj.descripcion,
                                                                                                  obj.precio,
                                                                                                  obj.usuario});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se ha registrado la Opcion del Evento de forma Exitosa";
                return result;
                
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al intentar registrar la Opcion del Evento";
                result.messageError = ex.ToString();
                return result;             
            }
        }

        public Response<OpcionEvento> ActualizarOpcionEvento(OpcionEvento obj)
        {
            Response<OpcionEvento> result = new Response<OpcionEvento>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al tratar de actualizar el registro";
            result.data = new OpcionEvento();

            if (obj.descripcion==null || obj.descripcion == "" ||
                obj.usuario==null || obj.usuario=="")
            {
                result.code = -1;
                result.data = new OpcionEvento();
                result.message = "Favor de verificar los campos requeridos, no es permitido Actualizar estos datos vacios";
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strUpdate = new StringBuilder();
                    strUpdate.Append(" update opcion_evento ");
                    strUpdate.Append(" set descripcion = upper(:descripcion), ");
                    strUpdate.Append(" precio = :precio, ");
                    strUpdate.Append(" usuario_modificacion = upper(:usuario), ");
                    strUpdate.Append(" fecha_modificacion = sysdate ");
                    strUpdate.Append(" where id_opcion = :id_opcion ");

                    var resp = db.Database.ExecuteSqlCommand(strUpdate.ToString(), new object[] { obj.descripcion,
                                                                                                  obj.precio,
                                                                                                  obj.usuario,
                                                                                                  obj.id});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se ha Actualizado el Registro de Opcion de Evento de forma exitosa";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una Excepcion al intentar actualizar la Opcion de Evento";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<OpcionEvento> EliminarOpcionEvento(OpcionEvento obj)
        {
            Response<OpcionEvento> result = new Response<OpcionEvento>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al tratar de eliminar el registro";
            result.data = new OpcionEvento();

            if (obj.usuario == null || obj.usuario == "")
            {
                result.code = -1;
                result.data = new OpcionEvento();
                result.message = "Favor de verificar los campos requeridos, no es permitido Eliminar el registro si estos datos estan vacios";
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strUpdateEstado = new StringBuilder();
                    strUpdateEstado.Append(" update opcion_evento ");
                    strUpdateEstado.Append(" set estado_registro = 'B', ");
                    strUpdateEstado.Append(" usuario_modificacion = upper(:usuario), ");
                    strUpdateEstado.Append(" fecha_modificacion = sysdate ");
                    strUpdateEstado.Append(" where id_opcion = :id_opcion ");


                    var resp = db.Database.ExecuteSqlCommand(strUpdateEstado.ToString(), new object[] { obj.usuario,
                                                                                                  obj.id});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se ha Eliminado el Registro de Opcion de Evento de forma exitosa";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una Excepcion al intentar Eliminar la Opcion de Evento";
                result.messageError = ex.ToString();
                return result;
            }
        }

    }
}