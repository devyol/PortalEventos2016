using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UI.Entidades;
using UI.Data;

namespace UI.Models
{
    public class GestionCobro
    {
        public Response<List<SaldoEvento>> SaldosEventos(Participante obj)
        {
            Response<List<SaldoEvento>> result = new Response<List<SaldoEvento>>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al tratar de obtener el Saldo de los Eventos";
            result.data = new List<SaldoEvento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strSl = new StringBuilder();
                    strSl.Append(" select inscripcionevento.id_participante, inscripcionevento.id_evento, inscripcionevento.nombre_evento, to_char(inscripcionevento.fecha_inscripcion,'dd/mm/yyyy')fecha_inscripcion, ");
                    strSl.Append(" monto_total_evento monto_total_evento,  monto_abonado, monto_total_evento-monto_abonado saldo_pendiente ");
                    strSl.Append(" from( ");
                    strSl.Append(" select ic.id_participante, ic.id_evento, ev.nombre_evento, ic.fecha_inscripcion ");
                    strSl.Append(" from inscripcion ic, evento ev ");
                    strSl.Append(" where ic.id_evento = ev.id_evento ");
                    strSl.Append(" and ic.id_participante = :id_participante) inscripcionevento, ");
                    strSl.Append(" ( ");
                    strSl.Append(" select insc.id_participante, insc.id_evento, nvl(inscripcionopcion.monto_total_evento,0)monto_total_evento ");
                    strSl.Append(" from (select iop.id_participante, iop.id_evento, sum(op.precio) monto_total_evento ");
                    strSl.Append(" from inscripcion_opcion iop, opcion_evento op ");
                    strSl.Append(" where iop.id_evento = op.id_evento ");
                    strSl.Append(" and iop.id_opcion = op.id_opcion ");
                    strSl.Append(" and iop.estado_registro = 'A' ");
                    strSl.Append(" and iop.id_participante = :id_participante ");
                    strSl.Append(" group by iop.id_participante, iop.id_evento)inscripcionopcion, inscripcion insc ");
                    strSl.Append(" where insc.id_participante = inscripcionopcion.id_participante(+) ");
                    strSl.Append(" and insc.id_evento = inscripcionopcion.id_evento(+) ");
                    strSl.Append(" and insc.id_participante = :id_participante) inscripcionopcion, ");
                    strSl.Append(" ( ");
                    //strSl.Append(" select insc.id_participante, insc.id_evento, nvl(sum(mov.valor),0) monto_abonado ");
                    //strSl.Append(" from inscripcion insc, movimientos mov ");
                    //strSl.Append(" where insc.id_participante = mov.id_participante(+) ");
                    //strSl.Append(" and insc.id_evento = mov.id_evento(+) ");
                    //strSl.Append(" and insc.id_participante = :id_participante ");
                    //strSl.Append(" group by insc.id_participante, insc.id_evento) inscripcionmovimiento ");
                    strSl.Append(" select abono.id_participante, abono.id_evento, abono.total_abono-cargo.total_cargo monto_abonado ");
                    strSl.Append(" from( ");
                    strSl.Append(" select insc.id_participante, insc.id_evento, nvl(sum(mov.valor),0) total_abono ");
                    strSl.Append(" from inscripcion insc, movimientos mov ");
                    strSl.Append(" where insc.id_participante = mov.id_participante(+) ");
                    strSl.Append(" and insc.id_evento = mov.id_evento(+) ");
                    strSl.Append(" and mov.tipo_mov(+) = 'AB' ");
                    strSl.Append(" and insc.id_participante = :id_participante ");
                    strSl.Append(" group by insc.id_participante, insc.id_evento) abono, ");
                    strSl.Append(" ( ");
                    strSl.Append(" select insc.id_participante, insc.id_evento, nvl(sum(mov.valor),0) total_cargo ");
                    strSl.Append(" from inscripcion insc, movimientos mov ");
                    strSl.Append(" where insc.id_participante = mov.id_participante(+) ");
                    strSl.Append(" and insc.id_evento = mov.id_evento(+) ");
                    strSl.Append(" and mov.tipo_mov(+) = 'CR' ");
                    strSl.Append(" and insc.id_participante = :id_participante ");
                    strSl.Append(" group by insc.id_participante, insc.id_evento) cargo ");
                    strSl.Append(" where abono.id_participante = cargo.id_participante ");
                    strSl.Append(" and abono.id_evento = cargo.id_evento) inscripcionmovimiento ");
                    strSl.Append(" where inscripcionevento.id_participante = inscripcionopcion.id_participante(+) ");
                    strSl.Append(" and inscripcionevento.id_evento = inscripcionopcion.id_evento(+) ");
                    strSl.Append(" and inscripcionevento.id_participante = inscripcionmovimiento.id_participante(+) ");
                    strSl.Append(" and inscripcionevento.id_evento = inscripcionmovimiento.id_evento(+) ");
                    strSl.Append(" order by 2 desc ");



                    var list = db.Database.SqlQuery<SaldoEvento>(strSl.ToString(), new object[] { obj.id,obj.id,obj.id,obj.id,obj.id }).ToList<SaldoEvento>();

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
                result.message = "Ocurrio una excepcion al tratar de obtener los datos del saldo de eventos";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<SaldoEvento> SaldoEvento(SaldoEvento obj)
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


                    var list = db.Database.SqlQuery<SaldoEvento>(strSE.ToString(), new object[] { obj.id_participante, 
                                                                                                  obj.id_evento,
                                                                                                  obj.id_participante,
                                                                                                  obj.id_participante,
                                                                                                  obj.id_participante,
                                                                                                  obj.id_participante}).SingleOrDefault<SaldoEvento>();
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

        public Response<Movimiento> AbonarSaldo(Movimiento obj)
        {
            Response<Movimiento> result = new Response<Movimiento>();
            result.code = -1;
            result.message = "Ocurrio un error en base de datos al tratar de Realizar el Pago";
            result.data = new Movimiento();
            

            if (obj.tipo_pago.Equals(null) || obj.valor.Equals(null) || obj.valor == 0 || 
                obj.usuario == null || obj.usuario == "")
            {
                result.code = -1;
                result.message = "Favor de revisar los valores Tipo de pago, Valor del pago o Usuario de Gestion que no sean vacios o tengan un valor de 0";
                result.data = new Movimiento();
                return result;
            }

            if (obj.saldo_pendiente == 0)
            {
                result.code = -1;
                result.message = "Para este Evento ya no es permito realizar Cobros ya que tiene Pagado el Monto total del Evento";
                result.data = new Movimiento();
                return result;
            }

            if ((obj.monto_abonado + obj.valor) > obj.monto_total_evento)
            {                
                result.code = -1;
                result.message = "Favor de verificar el monto a cobrar ya que excede al monto total del Evento, el monto valido a cobrar es Q " + obj.saldo_pendiente.ToString();
                result.data = new Movimiento();
                return result;

            }

            try
            {
                using (var db = new EntitiesEvento())
                {

                    StringBuilder strVal = new StringBuilder();
                    strVal.Append(" select nvl(tieneopcion.estado_registro,'N') estado_registro, estaasignado.tienebus ");
                    strVal.Append(" from ( ");
                    strVal.Append(" select io.id_participante, io.id_evento, nvl(io.estado_registro,'N')estado_registro ");
                    strVal.Append(" from opcion_evento oe, inscripcion_opcion io ");
                    strVal.Append(" where oe.id_evento = io.id_evento ");
                    strVal.Append(" and oe.id_opcion = io.id_opcion ");
                    strVal.Append(" and oe.es_transporte = 'S' ");
                    strVal.Append(" and io.id_evento = :id_evento ");
                    strVal.Append(" and io.id_participante = :id_participante) tieneopcion, ");
                    strVal.Append(" ( ");
                    strVal.Append(" select ins.id_participante, ins.id_evento, nvl(count(bi.id_bus),0)tienebus ");
                    strVal.Append(" from inscripcion ins, bus_inscripcion bi ");
                    strVal.Append(" where ins.id_evento = bi.id_evento(+) ");
                    strVal.Append(" and ins.id_participante = bi.id_participante(+) ");
                    strVal.Append(" and ins.id_evento = :id_evento ");
                    strVal.Append(" and ins.id_participante = :id_participante ");
                    strVal.Append(" group by ins.id_participante, ins.id_evento) estaasignado ");
                    strVal.Append(" where tieneopcion.id_participante(+) = estaasignado.id_participante ");
                    strVal.Append(" and tieneopcion.id_evento (+)= estaasignado.id_evento ");



                    var resultado = db.Database.SqlQuery<BusAsignacion>(strVal.ToString(), new object[] { obj.id_evento,
                                                                                                          obj.id_participante,
                                                                                                          obj.id_evento,
                                                                                                          obj.id_participante}).SingleOrDefault<BusAsignacion>();

                    if (resultado.tienebus == 0 && resultado.estado_registro == "A" )
                    {
                        result.code = -1;
                        result.message = "Antes de realizar el cobro, favor de proceder a la asignacion del Bus. Gracias";
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
                    strIns.Append(" 'AB', ");
                    strIns.Append(" :tipo_pago, ");
                    strIns.Append(" 'Pago al Evento '|| (select ev.nombre_evento from evento ev where id_evento = :id_evento), ");
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
                result.message = "Se ha registrado el abono de forma existosa";
                return result;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.message = "Ocurrio una excepcion al tratar de registrar el abono";
                result.messageError = ex.ToString();
                return result;
            }
        }

        public Response<List<Movimiento>> ListarMovimientosEvento(SaldoEvento obj)
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
                    strList.Append(" and mov.tipo_mov = 'AB'");
                    strList.Append(" and mov.id_evento = :id_evento ");
                    strList.Append(" order by mov.id_movimiento desc ");

                    var list = db.Database.SqlQuery<Movimiento>(strList.ToString(), new object[] { obj.id_participante,
                                                                                                   obj.id_evento}).ToList<Movimiento>();
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
    }
}