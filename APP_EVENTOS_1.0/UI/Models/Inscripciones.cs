using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UI.Entidades;
using UI.Data;
using System.Configuration;

namespace UI.Models
{
    public class Inscripciones
    {
        public Response<List<Evento>> EventosporInscribir(Participante obj)
        {
            Response<List<Evento>> result = new Response<List<Evento>>();
            result.code = -1;
            result.message = "Ocurrio un error en la base de datos y no se pudieron obtener los eventos para inscribirse";
            result.data = new List<Evento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strList = new StringBuilder();
                    strList.Append(" select distinct(ev.id_evento), ev.nombre_evento ");
                    strList.Append(" from evento ev, opcion_evento op ");
                    strList.Append(" where ev.id_evento = op.id_evento ");
                    strList.Append(" and ev.estado_registro = 'A' ");
                    strList.Append(" and op.estado_registro = 'A' ");
                    strList.Append(" and not exists ( ");
                    strList.Append(" select 1 ");
                    strList.Append(" from inscripcion ins ");
                    strList.Append(" where ins.id_evento = ev.id_evento ");
                    strList.Append(" and ins.id_participante =:id_participante) ");
                    strList.Append(" order by ev.id_evento desc");

                    var list = db.Database.SqlQuery<Evento>(strList.ToString(), new object[] { obj.id }).ToList<Evento>();
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
                result.message = "Ocurrio una excepcion al tratar de obtener los eventos disponibles a inscribir";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<Inscripcion> InsertarInscripcion(Inscripcion obj)
        {
            Response<Inscripcion> result = new Response<Inscripcion>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al tratar de realizar la inscripcion";
            result.data = new Inscripcion();

            if (obj.id_evento.Equals(null) || obj.id_evento == 0 ||
                obj.id_participante.Equals(null) || 
                obj.usuario == null || obj.usuario == "")
            {
                result.code = -1;
                result.data = new Inscripcion();
                result.message = "Favor de verificar los campos requeridos, no es valido guardar datos vacios";
                return result;
            }


            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strIns = new StringBuilder();                    
                    strIns.Append(" insert into inscripcion  ");
                    strIns.Append(" (id_evento,id_participante,fecha_inscripcion, estado_registro, usuario_creacion, fecha_creacion) ");
                    strIns.Append(" values( ");
                    strIns.Append(" :id_evento, ");
                    strIns.Append(" :id_participante, ");
                    strIns.Append(" sysdate, ");
                    strIns.Append(" 'A', ");
                    strIns.Append(" upper(:usuario), ");
                    strIns.Append(" sysdate) ");

                    var resp = db.Database.ExecuteSqlCommand(strIns.ToString(), new object[] { obj.id_evento, 
                                                                                               obj.id_participante,
                                                                                               obj.usuario });
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se ha Inscrito el Participante de forma correcta";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al tratar de Inscribir al Participante";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<List<Inscripcion>> EventosInscritos(Participante obj)
        {
            Response<List<Inscripcion>> result = new Response<List<Inscripcion>>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al tratar de obtener los Eventos Inscritos del Participante";
            result.data = new List<Inscripcion>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strEvIn = new StringBuilder();                    
                    strEvIn.Append(" select ins.id_participante, ins.id_evento, ev.nombre_evento, to_char(ins.fecha_inscripcion,'dd/mm/yyyy') fecha_inscripcion ");
                    strEvIn.Append(" from inscripcion ins, evento ev ");
                    strEvIn.Append(" where ins.id_evento = ev.id_evento ");
                    strEvIn.Append(" and ins.id_participante = :id_participante ");
                    strEvIn.Append(" order by 2 desc ");

                    var list = db.Database.SqlQuery<Inscripcion>(strEvIn.ToString(), new object[] { obj.id }).ToList<Inscripcion>();
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
                result.message = "Ocurrio una excepcion al tratar se obtener el listado de Eventos Inscritos";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<List<InscripcionOpcion>> OpcionesInscrito(Inscripcion obj)
        {
            Response<List<InscripcionOpcion>> result = new Response<List<InscripcionOpcion>>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al obtener las opciones del Inscrito";
            result.data = new List<InscripcionOpcion>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strOpIn = new StringBuilder();
                    strOpIn.Append(" select inop.id_participante, inop.id_evento, inop.id_opcion, inop.estado_registro estado, ");
                    strOpIn.Append(" opev.descripcion opcion, opev.precio, opev.obligatorio, opev.es_transporte  ");
                    strOpIn.Append(" from inscripcion_opcion inop, opcion_evento opev ");
                    strOpIn.Append(" where inop.id_evento = opev.id_evento ");
                    strOpIn.Append(" and inop.id_opcion = opev.id_opcion ");
                    //strOpIn.Append(" and inop.estado_registro = 'A' ");
                    strOpIn.Append(" and inop.id_participante = :id_participante ");
                    strOpIn.Append(" and inop.id_evento = :id_evento ");

                    var list = db.Database.SqlQuery<InscripcionOpcion>(strOpIn.ToString(), new object[] { obj.id_participante, obj.id_evento }).ToList<InscripcionOpcion>();

                    foreach (var item in list)
                    {
                        if (item.estado == "A")
                        {
                            item.estador = new Estado();
                            item.estador.estado = "A";
                            item.estador.descripcion = "AGREGADA";                            
                        }

                        if (item.estado == "B")
                        {
                            item.estador = new Estado();
                            item.estador.estado = "B";
                            item.estador.descripcion = "NO AGREGADA";                            
                        }

                        if (item.obligatorio == "S")
                        {
                            item.estadoOpcion = true;
                        }

                        if (item.obligatorio == "N")
                        {
                            item.estadoOpcion = false;
                        }

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
                result.message = "Ocurrio una excepcion al tratar de obtener las opciones del inscrito";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<InscripcionOpcion> ActualizarOpcionInscripcion(InscripcionOpcion obj)
        {
            Response<InscripcionOpcion> result = new Response<InscripcionOpcion>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al tratar de modificar la opcion de la inscripción";
            result.data = new InscripcionOpcion();

            DateTime fechalimite = Convert.ToDateTime(ConfigurationManager.AppSettings["FechaLimite"].ToString());

            try
            {
                using (var db = new EntitiesEvento())
                {
                    string newEstado = "";
                    
                    StringBuilder strUpEs = new StringBuilder();
                    strUpEs.Append(" update inscripcion_opcion ");
                    strUpEs.Append(" set estado_registro = :estado_registro ");
                    strUpEs.Append(" where id_participante = :id_participante ");
                    strUpEs.Append(" and id_evento = :id_evento ");
                    strUpEs.Append(" and id_opcion = :id_opcion ");

                    if (obj.es_transporte == "S" && (fechalimite < DateTime.Today) && obj.estador.estado == "A")
                    {
                        result.code = -1;
                        result.message = "No es posible quitar el transporte, la fecha limite fue el " + fechalimite.ToString();
                        return result;
                    }

                    if (obj.estador.estado == "A")
                    {
                        newEstado = "B";
                    }
                    else
                    {
                        newEstado = "A";
                    }

                    var resp = db.Database.ExecuteSqlCommand(strUpEs.ToString(), new object[] { newEstado,
                                                                                                obj.id_participante,
                                                                                                obj.id_evento,
                                                                                                obj.id_opcion});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se actualizo el estado correctamente";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepción al tratar de actualizar el estado de la opcion de inscripcion";
                result.messageError = ex.ToString();
                return result;
            }
        }
    }
}