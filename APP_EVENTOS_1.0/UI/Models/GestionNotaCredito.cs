using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UI.Entidades;
using UI.Data;

namespace UI.Models
{
    public class GestionNotaCredito
    {
        /// <summary>
        /// LISTA LOS EVENTOS ACTIVOS PARA LLENAR COMBO EN HTML
        /// </summary>
        /// <returns></returns>
        public Response<List<Evento>> ListaEventoActivos()
        {
            Response<List<Evento>> result = new Response<List<Evento>>();
            result.code = -1;
            result.message = "Ocurrio un Error en Base de Datos al obtener el Listado de Eventos";
            result.data = new List<Evento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strList = new StringBuilder();
                    
                    strList.Append(" select id_evento, nombre_evento ");
                    strList.Append(" from evento ");
                    strList.Append(" where estado_registro = 'A' ");
                    strList.Append(" order by id_evento desc ");

                    var list = db.Database.SqlQuery<Evento>(strList.ToString()).ToList<Evento>();

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
                result.message = "Ocurrio una Excepcion al tratar de obtener el listado de Eventos";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<List<Participante>> ListaParticipantes(decimal evento)
        {
            Response<List<Participante>> result = new Response<List<Participante>>();
            result.code = -1;
            result.message = "Ocurrio un Error en Base de Datos al tratar de Obtner el Listado de Participantes";
            result.data = new List<Participante>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strList = new StringBuilder();
                    
                    strList.Append(" select insc.id_participante id, insc.id_evento idevento, pa.nombre||' '||pa.apellido nombrec ");
                    strList.Append(" from inscripcion insc, participante pa ");
                    strList.Append(" where insc.id_participante = pa.id_participante ");
                    strList.Append(" and pa.estado_registro = 'A' ");
                    strList.Append(" and insc.id_evento = :id_evento ");
                    strList.Append(" order by nombrec ");

                    var list = db.Database.SqlQuery<Participante>(strList.ToString(), new object[] { evento }).ToList<Participante>();

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
                result.message = "Ocurrio una Excepcion al tratar de Obtener el Listado de Participantes";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<SaldoEvento> SaldoEventoNota(Participante obj)
        {
            Response<SaldoEvento> result = new Response<SaldoEvento>();
            result.code = -1;
            result.message = "Se genero un error en base de datos al tratar de Obtener el registro del saldo del Evento Inscrito";
            result.data = new SaldoEvento();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strSE = new StringBuilder();
                    strSE.Append(" select inscripcionevento.id_participante, inscripcionevento.id_evento, inscripcionevento.nombre_evento, to_char(inscripcionevento.fecha_inscripcion,'dd/mm/yyyy')fecha_inscripcion, ");
                    strSE.Append(" monto_total_evento monto_total_evento,  monto_abonado, monto_total_evento-monto_abonado saldo_pendiente ");
                    strSE.Append(" from( ");
                    strSE.Append(" select ic.id_participante, ic.id_evento, ev.nombre_evento, ic.fecha_inscripcion ");
                    strSE.Append(" from inscripcion ic, evento ev ");
                    strSE.Append(" where ic.id_evento = ev.id_evento ");
                    strSE.Append(" and ic.id_participante = :id_participante ");
                    strSE.Append(" and ic.id_evento = :id_evento) inscripcionevento, ");
                    strSE.Append(" ( ");
                    strSE.Append(" select insc.id_participante, insc.id_evento, nvl(inscripcionopcion.monto_total_evento,0)monto_total_evento ");
                    strSE.Append(" from (select iop.id_participante, iop.id_evento, sum(op.precio) monto_total_evento ");
                    strSE.Append(" from inscripcion_opcion iop, opcion_evento op ");
                    strSE.Append(" where iop.id_evento = op.id_evento ");
                    strSE.Append(" and iop.id_opcion = op.id_opcion ");
                    strSE.Append(" and iop.estado_registro = 'A' ");
                    strSE.Append(" and iop.id_participante = :id_participante ");
                    strSE.Append(" group by iop.id_participante, iop.id_evento)inscripcionopcion, inscripcion insc ");
                    strSE.Append(" where insc.id_participante = inscripcionopcion.id_participante(+) ");
                    strSE.Append(" and insc.id_evento = inscripcionopcion.id_evento(+) ");
                    strSE.Append(" and insc.id_participante = :id_participante) inscripcionopcion, ");
                    strSE.Append(" ( ");
                    //strSE.Append(" select insc.id_participante, insc.id_evento, nvl(sum(mov.valor),0) monto_abonado ");
                    //strSE.Append(" from inscripcion insc, movimientos mov ");
                    //strSE.Append(" where insc.id_participante = mov.id_participante(+) ");
                    //strSE.Append(" and insc.id_evento = mov.id_evento(+) ");
                    //strSE.Append(" and insc.id_participante = :id_participante ");
                    //strSE.Append(" group by insc.id_participante, insc.id_evento) inscripcionmovimiento ");
                    strSE.Append(" select abono.id_participante, abono.id_evento, abono.total_abono-cargo.total_cargo monto_abonado ");
                    strSE.Append(" from( ");
                    strSE.Append(" select insc.id_participante, insc.id_evento, nvl(sum(mov.valor),0) total_abono ");
                    strSE.Append(" from inscripcion insc, movimientos mov ");
                    strSE.Append(" where insc.id_participante = mov.id_participante(+) ");
                    strSE.Append(" and insc.id_evento = mov.id_evento(+) ");
                    strSE.Append(" and mov.tipo_mov(+) = 'AB' ");
                    strSE.Append(" and insc.id_participante = :id_participante ");
                    strSE.Append(" group by insc.id_participante, insc.id_evento) abono, ");
                    strSE.Append(" ( ");
                    strSE.Append(" select insc.id_participante, insc.id_evento, nvl(sum(mov.valor),0) total_cargo ");
                    strSE.Append(" from inscripcion insc, movimientos mov ");
                    strSE.Append(" where insc.id_participante = mov.id_participante(+) ");
                    strSE.Append(" and insc.id_evento = mov.id_evento(+) ");
                    strSE.Append(" and mov.tipo_mov(+) = 'CR' ");
                    strSE.Append(" and insc.id_participante = :id_participante ");
                    strSE.Append(" group by insc.id_participante, insc.id_evento) cargo ");
                    strSE.Append(" where abono.id_participante = cargo.id_participante ");
                    strSE.Append(" and abono.id_evento = cargo.id_evento) inscripcionmovimiento ");
                    strSE.Append(" where inscripcionevento.id_participante = inscripcionopcion.id_participante(+) ");
                    strSE.Append(" and inscripcionevento.id_evento = inscripcionopcion.id_evento(+) ");
                    strSE.Append(" and inscripcionevento.id_participante = inscripcionmovimiento.id_participante(+) ");
                    strSE.Append(" and inscripcionevento.id_evento = inscripcionmovimiento.id_evento(+) ");
                    strSE.Append(" order by 2 desc ");


                    var list = db.Database.SqlQuery<SaldoEvento>(strSE.ToString(), new object[] { obj.id, 
                                                                                                  obj.idevento,
                                                                                                  obj.id,
                                                                                                  obj.id,
                                                                                                  obj.id,
                                                                                                  obj.id}).SingleOrDefault<SaldoEvento>();
                    if (list == null)
                    {
                        result.code = -1;
                        result.message = "No existe registro de Saldos de Evento";
                        result.data = new SaldoEvento();
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
                result.message = "Ocurrio una excepcion al tratar de obtener la informacion del Saldo del Evento";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<List<Movimiento>> ListarCargosNota(Participante obj)
        {
            Response<List<Movimiento>> result = new Response<List<Movimiento>>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al tratar de obtener listado de Movimientos del Evento Inscrito";
            result.data = new List<Movimiento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strList = new StringBuilder();
                    strList.Append(" select  mov.id_movimiento, mov.id_participante, mov.id_evento,  ");
                    strList.Append(" tp.descripcion tipo_pago_d, mov.descripcion, mov.valor,  ");
                    strList.Append(" to_char(mov.fecha_movimiento,'dd/mm/yyyy') fecha_movimiento, mov.usuario_creacion usuario");
                    strList.Append(" from movimientos mov, tipo_pago tp ");
                    strList.Append(" where mov.tipo_pago = tp.tipo_pago ");
                    strList.Append(" and mov.id_participante = :id_participante ");
                    strList.Append(" and mov.tipo_mov = 'CR'");
                    strList.Append(" and mov.id_evento = :id_evento ");
                    strList.Append(" order by mov.id_movimiento desc ");

                    var list = db.Database.SqlQuery<Movimiento>(strList.ToString(), new object[] { obj.id,
                                                                                                   obj.idevento}).ToList<Movimiento>();
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
                result.message = "Ocurrio una excepcion al tratar de obtener el listado de Movimientos del Evento Inscrito";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<Movimiento> GenerarNotaDeCredito(Movimiento obj)
        {
            Response<Movimiento> result = new Response<Movimiento>();
            result.code = -1;
            result.message = "Ocurrio un Error en Base de Datos al tratar de Generar la Nota de Crédito";
            result.data = new Movimiento();

            if (obj.tipo_pago.Equals(null) || obj.valor.Equals(null) || obj.valor == 0 ||
                obj.usuario == null || obj.usuario == "")
            {
                result.code = -1;
                result.message = "Favor de revisar los valores Tipo de pago, Valor del pago o Usuario de Gestion que no sean vacios o tengan un valor de 0";
                result.data = new Movimiento();
                return result;
            }

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strVal = new StringBuilder();                    
                    //strVal.Append(" select sum(valor) monto_abonado ");
                    //strVal.Append(" from movimientos ");
                    //strVal.Append(" where tipo_mov = 'AB' ");
                    //strVal.Append(" and id_evento = :id_evento ");
                    //strVal.Append(" and id_participante = :id_participante ");

                    strVal.Append(" select (abono.abonos-cargo.cargos) monto_abonado ");
                    strVal.Append(" from( ");
                    strVal.Append(" select ins.id_participante, ins.id_evento, nvl(sum(mov.valor),0) abonos ");
                    strVal.Append(" from movimientos mov, inscripcion ins ");
                    strVal.Append(" where mov.id_evento(+) = ins.id_evento ");
                    strVal.Append(" and mov.id_participante(+) = ins.id_participante ");
                    strVal.Append(" and mov.tipo_mov(+) = 'AB'  ");
                    strVal.Append(" and ins.id_evento = :id_evento  ");
                    strVal.Append(" and ins.id_participante = :id_participante ");
                    strVal.Append(" group by ins.id_evento, ins.id_participante) abono, ");
                    strVal.Append(" ( ");
                    strVal.Append(" select ins.id_participante, ins.id_evento, nvl(sum(mov.valor),0) cargos ");
                    strVal.Append(" from movimientos mov, inscripcion ins ");
                    strVal.Append(" where mov.id_evento(+) = ins.id_evento ");
                    strVal.Append(" and mov.id_participante(+) = ins.id_participante ");
                    strVal.Append(" and mov.tipo_mov(+) = 'CR'  ");
                    strVal.Append(" and ins.id_evento = :id_evento  ");
                    strVal.Append(" and ins.id_participante = :id_participante ");
                    strVal.Append(" group by ins.id_evento, ins.id_participante) cargo ");
                    strVal.Append(" where abono.id_participante = cargo.id_participante(+) ");
                    strVal.Append(" and abono.id_evento = cargo.id_evento(+) ");


                    var valida = db.Database.SqlQuery<Movimiento>(strVal.ToString(), new object[] { obj.id_evento, 
                                                                                                    obj.id_participante,
                                                                                                    obj.id_evento, 
                                                                                                    obj.id_participante,}).SingleOrDefault<Movimiento>();

                    if (valida.monto_abonado==0)
                    {
                        result.code = -1;
                        result.message = "No es posible generar Nota de Crédito ya que no posee ningun Saldo Abonado para este Evento";
                        result.data = new Movimiento();
                        return result;
                    }

                    if ( obj.valor > valida.monto_abonado)
                    {
                        result.code = -1;
                        result.message = "El Valor por el cual desea realizar la Nota de Crédito excede al Valor Abonado para este Evento, verifique el Saldo Abonado";
                        result.data = new Movimiento();
                        return result;
                    }

                    StringBuilder strIns = new StringBuilder();
                    strIns.Append(" insert into movimientos ");
                    strIns.Append(" (id_movimiento,id_participante,id_evento, tipo_mov, tipo_pago, descripcion, valor, fecha_movimiento, usuario_creacion, fecha_creacion) ");
                    strIns.Append(" values( ");
                    strIns.Append(" (select correlativo_disponible from correlativo where id_correlativo = 7 and estado_registro = 'A'), ");
                    strIns.Append(" :id_participante, ");
                    strIns.Append(" :id_evento, ");
                    strIns.Append(" 'CR', ");
                    strIns.Append(" :tipo_pago, ");
                    strIns.Append(" 'Nota de Crédito que Afecta al Evento:  '|| (select ev.nombre_evento from evento ev where ev.id_evento = :id_evento), ");
                    strIns.Append(" :valor, ");
                    strIns.Append(" sysdate, ");
                    strIns.Append(" upper(:usuario), ");
                    strIns.Append(" sysdate) ");

                    var resp = db.Database.ExecuteSqlCommand(strIns.ToString(), new object[] { obj.id_participante, 
                                                                                               obj.id_evento,
                                                                                               obj.tipo_pago,
                                                                                               obj.id_evento,
                                                                                               obj.valor,
                                                                                               obj.usuario});
                    db.SaveChanges();                    
                }
                result.code = 0;
                result.message = "Se ha generado la Nota de Crédito de forma existosa";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al tratar de registrar la Nota de Crédito";
                result.messageError = ex.ToString();
                return result;
            }
        }        

    }
}