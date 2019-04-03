using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UI.Entidades;
using UI.Data;

namespace UI.Models
{
    public class GestionBusInscripcion
    {
        /// <summary>
        /// LISTADO DE EVENTOS QUE TIENEN UNA OPCION DE BUS
        /// </summary>
        /// <returns></returns>
        public Response<List<Evento>> EventosConBus()
        {
            Response<List<Evento>> result = new Response<List<Evento>>();
            result.code = -1;
            result.message = "Ocurrio un Error en Base de Datos al tratar de Obtener el Listado de Eventos";
            result.data = new List<Evento>();
            result.totalRecords = 0;

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strSQL = new StringBuilder();
                    
                    strSQL.Append(" select distinct ev.id_evento, ev.nombre_evento ");
                    strSQL.Append(" from evento ev, bus_evento be ");
                    strSQL.Append(" where ev.id_evento = be.id_evento ");
                    strSQL.Append(" and ev.estado_registro = 'A' ");

                    var list = db.Database.SqlQuery<Evento>(strSQL.ToString()).ToList<Evento>();

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
                result.message = "Ocurrio una Excepcion al tratar de obtener el Listado de Eventos";
                result.messageError = ex.ToString();
                return result;
            }
        }

        /// <summary>
        /// LISTADO DE PARTICIPANTES QUE TIENEN LA OPCION DE TRANSPORTE
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        public Response<List<Participante>> ParticipantesPorEvento(decimal evento)
        {
            Response<List<Participante>> result = new Response<List<Participante>>();
            result.code = -1;
            result.message = "Ocurrio un Error en Base de datos al tratar de obtener el Listado de Participantes";
            result.data = new List<Participante>();
            result.totalRecords = 0;

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strSQL = new StringBuilder();

                    strSQL.Append(" select insc.id_participante id, insc.id_evento idevento, pa.nombre||' '||pa.apellido nombrec  ");
                    strSQL.Append(" from inscripcion insc, participante pa, inscripcion_opcion inop, opcion_evento oe ");
                    strSQL.Append(" where insc.id_participante = pa.id_participante  ");
                    strSQL.Append(" and insc.id_participante = inop.id_participante ");
                    strSQL.Append(" and insc.id_evento = inop.id_evento ");
                    strSQL.Append(" and inop.id_evento = oe.id_evento ");
                    strSQL.Append(" and inop.id_opcion = oe.id_opcion ");
                    strSQL.Append(" and oe.es_transporte = 'S' ");
                    strSQL.Append(" and inop.estado_registro = 'A' ");
                    strSQL.Append(" and pa.estado_registro = 'A' ");
                    strSQL.Append(" and insc.id_evento = :id_evento ");
                    strSQL.Append(" order by pa.apellido ");

                    var list = db.Database.SqlQuery<Participante>(strSQL.ToString(), new object[] { evento }).ToList<Participante>();

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
                result.message = "Ocurrio una Excepcion al momento de Obtener el Listado de Participantes";
                result.messageError = ex.ToString();
                return result;
            }
        }

        /// <summary>
        /// SE OBTIENE EL DATO UNICO DE UN PARTICIPANTE CON TRANSPORTE
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public Response<Participante> ParticipanteConTransporte(Participante par)
        {
            Response<Participante> result = new Response<Participante>();
            result.code = -1;
            result.message = "Ocurrio una Error en Base de datos al tratar de obtener el Participantes con Bus";
            result.data = new Participante();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strPa = new StringBuilder();
                    strPa.Append(" select insc.id_participante id, insc.id_evento idevento, pa.nombre||' '||pa.apellido nombrec  ");
                    strPa.Append(" from inscripcion insc, participante pa, inscripcion_opcion inop, opcion_evento oe ");
                    strPa.Append(" where insc.id_participante = pa.id_participante  ");
                    strPa.Append(" and insc.id_participante = inop.id_participante ");
                    strPa.Append(" and insc.id_evento = inop.id_evento ");
                    strPa.Append(" and inop.id_evento = oe.id_evento ");
                    strPa.Append(" and inop.id_opcion = oe.id_opcion ");
                    strPa.Append(" and oe.es_transporte = 'S' ");
                    strPa.Append(" and inop.estado_registro = 'A' ");
                    strPa.Append(" and insc.id_evento = :id_evento ");
                    strPa.Append(" and insc.id_participante = :id_participante ");
                    strPa.Append(" order by pa.apellido ");

                    var list = db.Database.SqlQuery<Participante>(strPa.ToString(), new object[] { par.idevento, par.id }).SingleOrDefault<Participante>();

                    if (list == null)
                    {
                        result.code = -1;
                        result.message = "No existe Evento Registrado";
                        result.data = new Participante();
                        result.totalRecords = 0;
                        return result;                        
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
                result.message = "Ocurrio una excepcion al tratar de obtener los datos del Participante";
                result.messageError = ex.ToString();
                return result;
            }
        }

        /// <summary>
        /// LISTADO DE BUSES DE UN EVENTO, ESTE METODO LLENA LA INFORMACION DE CONTEO DE DISPONIBILIDAD DE LOS BUSES
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        public Response<List<Bus>> BusesEvento(decimal evento)
        {
            Response<List<Bus>> result = new Response<List<Bus>>();
            result.code = -1;
            result.message = "Ocurrio un Error en Base de Datos al obtener los Buses";
            result.data = new List<Bus>();
            result.totalRecords = 0;

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strSQL = new StringBuilder();
                    strSQL.Append(" select id_evento, id_bus, no_bus, 'Bus No. '||to_char(no_bus) descripcion, capacidad, disponible, ocupado, hora_salida ");
                    strSQL.Append(" from bus_evento ");
                    strSQL.Append(" where id_evento = :id_evento ");
                    strSQL.Append(" and estado_registro = 'A' ");

                    var list = db.Database.SqlQuery<Bus>(strSQL.ToString(), new object[] { evento }).ToList<Bus>();

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
                result.message = "Ocurrio una Excepcion al tratar de obtener el listado de Buses";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<List<Bus>> BusesDisponiblesAsignar(Participante pa)
        {
            Response<List<Bus>> result = new Response<List<Bus>>();
            result.code = -1;
            result.message = "Ocurrio un error en Base de datos al tratar de Obtener listado de Buses Disponibles";
            result.data = new List<Bus>();
            result.totalRecords = 0;

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strBDis = new StringBuilder();
                    strBDis.Append(" select be.id_evento, be.id_bus, be.no_bus, 'Bus No. '||to_char(be.no_bus) descripcion, be.capacidad, be.disponible, be.ocupado, be.hora_salida ");
                    strBDis.Append(" from bus_evento be ");
                    strBDis.Append(" where be.id_evento = :id_evento ");
                    strBDis.Append(" and be.disponible != 0 ");
                    strBDis.Append(" and be.estado_registro = 'A' ");
                    strBDis.Append(" and 1 = ( ");
                    strBDis.Append(" select decode(count(1),0,1,0) resultado ");
                    strBDis.Append(" from bus_inscripcion bi ");
                    strBDis.Append(" where bi.id_evento =  be.id_evento ");
                    strBDis.Append(" and bi.id_participante = :id_participante) ");

                    var list = db.Database.SqlQuery<Bus>(strBDis.ToString(), new object[] { pa.idevento, pa.id }).ToList<Bus>();

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
                result.message = "Ocurrio una excepcion al tratar de obtener el listado de buses disponibles";
                result.messageError = ex.ToString();
                return result;
                
            }
        }

        public Response<List<BusInscripcion>> BusAsignado(Participante pa)
        {
            Response<List<BusInscripcion>> result = new Response<List<BusInscripcion>>();
            result.code = -1;
            result.message = "Ocurrio un error en Base de Datos al tratar de obtener la informacion de Bus Asignado";
            result.data = new List<BusInscripcion>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strBus = new StringBuilder();
                    strBus.Append(" select bi.id_bus, bi.id_participante, bi.id_evento, be.no_bus, 'Bus No. '||to_char(be.no_bus) descripcion, be.hora_salida ");
                    strBus.Append(" from bus_inscripcion bi, bus_evento be ");
                    strBus.Append(" where bi.id_evento = be.id_evento ");
                    strBus.Append(" and bi.id_bus = be.id_bus ");
                    strBus.Append(" and bi.id_evento = :id_evento ");
                    strBus.Append(" and bi.id_participante = :id_participante ");

                    var list = db.Database.SqlQuery<BusInscripcion>(strBus.ToString(), new object[] { pa.idevento, pa.id }).ToList<BusInscripcion>();

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
                result.message = "Ocurrio una excepcion al tratar de obtenr el Bus Asignado";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<BusInscripcion> BusAsignadoDatoUnico(Participante pa)
        {
            Response<BusInscripcion> result = new Response<BusInscripcion>();
            result.code = -1;
            result.message = "Ocurrio un error en Base de Datos al tratar de obtener la informacion de Bus Asignado";
            result.data = new BusInscripcion();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strBus = new StringBuilder();
                    strBus.Append(" select bi.id_bus, bi.id_participante, bi.id_evento, be.no_bus, 'Bus No. '||to_char(be.no_bus) descripcion, be.hora_salida ");
                    strBus.Append(" from bus_inscripcion bi, bus_evento be ");
                    strBus.Append(" where bi.id_evento = be.id_evento ");
                    strBus.Append(" and bi.id_bus = be.id_bus ");
                    strBus.Append(" and bi.id_evento = :id_evento ");
                    strBus.Append(" and bi.id_participante = :id_participante ");

                    var list = db.Database.SqlQuery<BusInscripcion>(strBus.ToString(), new object[] { pa.idevento, pa.id }).SingleOrDefault<BusInscripcion>();

                    if (list == null)
                    {
                        result.code = -1;
                        result.message = "No existe informacion del Bus Asignado para el Participante Indicado";
                        result.data = new BusInscripcion();
                        result.totalRecords = 0;
                        return result;                        
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
                result.message = "Ocurrio una excepcion al tratar de obtenr el Bus Asignado";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<BusInscripcion> AsignarBusParticipante(BusInscripcion pa)
        {
            Response<BusInscripcion> result = new Response<BusInscripcion>();
            result.code = -1;
            result.message = "Ocurrio un error En Base de Datos al tratar de Asignar el Bus";
            result.data = new BusInscripcion();

            if (pa.no_bus.Equals(null) || pa.no_bus == 0 ||                
                pa.usuario == null || pa.usuario == "")
            {
                result.code = -1;
                result.data = new BusInscripcion();
                result.message = "Favor de verificar los campos de Bus y Usuario. Se requieren estos campos para Asignar el Bus";
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strVal = new StringBuilder();
                    strVal.Append(" select disponible ");
                    strVal.Append(" from bus_evento ");
                    strVal.Append(" where id_evento =:id_evento ");
                    strVal.Append(" and id_bus =:id_bus ");

                    var val = db.Database.SqlQuery<Bus>(strVal.ToString(), new object[] { pa.id_evento, pa.id_bus }).SingleOrDefault<Bus>();

                    if (val.disponible == 0)
                    {
                        result.code = -1;
                        result.message = "No es posible Asignar en este Bus ya que todos los ascientos estan Ocupados, Revise Disponibilidad en otro Bus";
                        result.data = new BusInscripcion();
                        return result;                        
                    }

                    StringBuilder insBus = new StringBuilder();                    
                    insBus.Append(" insert into bus_inscripcion(id_bus, id_participante, id_evento, no_bus, estado_registro, usuario_creacion, fecha_creacion) ");
                    insBus.Append(" values( ");
                    insBus.Append(" :id_bus, ");
                    insBus.Append(" :id_participante, ");
                    insBus.Append(" :id_evento, ");
                    insBus.Append(" :no_bus, ");
                    insBus.Append(" 'A', ");
                    insBus.Append(" upper(:usuario), ");
                    insBus.Append(" sysdate) ");

                    var resp = db.Database.ExecuteSqlCommand(insBus.ToString(), new object[]{pa.id_bus,
                                                                                             pa.id_participante,
                                                                                             pa.id_evento,
                                                                                             pa.no_bus,
                                                                                             pa.usuario});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se ha realizado la Asignacion de Bus de forma Exitosa";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al tratar de Asignar Bus al Participante";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<List<Bus>> BusesDisponiblesModificar(Participante pa)
        {
            Response<List<Bus>> result = new Response<List<Bus>>();
            result.code = -1;
            result.message = "Ocurrio un error en Base de datos al tratar de obtener el listado de Buses para Asignar";
            result.data = new List<Bus>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strSQL = new StringBuilder();

                    strSQL.Append(" select be.id_evento, be.id_bus, be.no_bus, 'Bus No. '||to_char(be.no_bus) descripcion, be.capacidad, be.disponible, be.ocupado, be.hora_salida ");
                    strSQL.Append(" from bus_evento be ");
                    strSQL.Append(" where be.disponible != 0 ");
                    strSQL.Append(" and be.estado_registro = 'A' ");
                    strSQL.Append(" and not exists( ");
                    strSQL.Append(" select 1 ");
                    strSQL.Append(" from ( ");
                    strSQL.Append(" select bi.id_evento, bi.id_bus ");
                    strSQL.Append(" from bus_evento bev, bus_inscripcion bi ");
                    strSQL.Append(" where bev.id_bus = bi.id_bus ");
                    strSQL.Append(" and bev.id_evento = bi.id_evento ");
                    strSQL.Append(" and bi.id_evento = :id_evento ");
                    strSQL.Append(" and bi.id_participante = :id_participante ");
                    strSQL.Append(" and bev.disponible != 0) buspart ");
                    strSQL.Append(" where buspart.id_evento = be.id_evento ");
                    strSQL.Append(" and buspart.id_bus = be.id_bus) ");


                    var list = db.Database.SqlQuery<Bus>(strSQL.ToString(), new object[] { pa.idevento, pa.id }).ToList<Bus>();

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
                result.message = "Ocurrio una excepcion al momento de obtener el Listado de Buses para Modificar";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<BusInscripcion> ModificarBus(BusInscripcion bu)
        {
            Response<BusInscripcion> result = new Response<BusInscripcion>();
            result.code = -1;
            result.message = "Ocurrio un error en Base de Datos al tratar de Modificar el Bus";
            result.data = new BusInscripcion();


            if (bu.no_bus_new.Equals(null) || bu.no_bus_new == 0 ||
                bu.usuario == null || bu.usuario == "")
            {
                result.code = -1;
                result.data = new BusInscripcion();
                result.message = "Favor de verificar los campos de Bus y Usuario. Se requieren estos campos para Modificar el Bus";
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder updBus = new StringBuilder();
                    updBus.Append("  ");
                    updBus.Append(" update bus_inscripcion ");
                    updBus.Append(" set no_bus =:no_bus_new, id_bus =:id_bus_new, usuario_modificacion =:usuario, fecha_modificacion = sysdate ");
                    updBus.Append(" where id_bus = :id_bus ");
                    updBus.Append(" and id_participante = :id_participante ");
                    updBus.Append(" and id_evento = :id_evento ");

                    var resp = db.Database.ExecuteSqlCommand(updBus.ToString(), new object[]{ bu.no_bus_new,
                                                                                             bu.id_bus_new,
                                                                                             bu.usuario,
                                                                                             bu.id_bus,
                                                                                             bu.id_participante,
                                                                                             bu.id_evento});
                    db.SaveChanges();
                }
                result.code = 0;
                result.message = "Se ha modificado el Bus del Participante exitosamente...";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al tratar de Modificar el Bus al Participante";
                result.messageError = ex.ToString();
                return result;
            }
        }

    }
}