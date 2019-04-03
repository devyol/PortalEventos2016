using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UI.Entidades;
using UI.Data;

namespace UI.Models
{
    public class BusInscripcion
    {

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
                    
                    strSQL.Append(" select insc.id_participante id, insc.id_evento idevento, pa.nombre||' '||pa.apellido nombrec ");
                    strSQL.Append(" from inscripcion insc, participante pa ");
                    strSQL.Append(" where insc.id_participante = pa.id_participante ");
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
                    strSQL.Append(" select id_evento, id_bus, no_bus, capacidad, disponible, ocupado, hora_salida ");
                    strSQL.Append(" from bus_evento ");
                    strSQL.Append(" where id_evento = :id_evento ");

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
    }
}