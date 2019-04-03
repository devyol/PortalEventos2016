using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UI.Entidades;
using UI.Data;

namespace UI.Models
{
    public class ReciboMovimiento
    {

        #region Propiedades

        //propiedades del recibo
        public decimal id_movimiento { get; set; }
        public decimal id_participante { get; set; }
        public decimal id_evento { get; set; }
        public string opcion { get; set; }
        public decimal precio { get; set; }
        public decimal valor { get; set; }
        public decimal numero_recibo { get; set; }
        public string fecha_movimiento { get; set; }
        public string no_bus { get; set; }
        public string hora_salida { get; set; }
        public string nota { get; set; }
        public string tipo_pago { get; set; }
        public decimal? total_abonos { get; set; }
        public decimal? total_cargos { get; set; }
        public decimal total { get; set; }
        public decimal id_opcion { get; set; }
        public string descripcion_opcion { get; set; }

        //propiedades del reporte
        public string nombre_evento { get; set; }
        public string nombre_participante { get; set; }
        public decimal monto_total_evento { get; set; }
        public decimal monto_abonado { get; set; }
        public decimal saldo_pendiente { get; set; }
        public string marca { get; set; }
        public string telefono { get; set; }
        public string talla { get; set; }
        public decimal? bus { get; set; }
        public string genero { get; set; }
        public string fecha_inscripcion { get; set; }
        public string ultimo_pago { get; set; }

        #endregion

        #region Constantes

        private const string _listadoporEvento = @"select principal.id_evento,principal.id_participante, principal.nombre_evento, principal.nombres nombre_participante, total.total_evento_participante monto_total_evento,
                                                    nvl(ab.valor_abono,0) - nvl(cr.valor_cargo,0) monto_abonado, total.total_evento_participante - (nvl(ab.valor_abono,0) - nvl(cr.valor_cargo,0)) saldo_pendiente
                                                    from(
                                                    select ins.id_evento, ins.id_participante, ev.nombre_evento, pa.nombre||' '||pa.apellido nombres
                                                    from inscripcion ins, participante pa, evento ev
                                                    where ins.id_participante = pa.id_participante
                                                    and ins.id_evento = ev.id_evento
                                                    and pa.estado_registro = 'A'
                                                    and ins.estado_registro = 'A'
                                                    )principal,
                                                    (
                                                    select iop.id_evento, iop.id_participante, sum(opev.precio)total_evento_participante
                                                    from inscripcion_opcion iop, opcion_evento opev
                                                    where iop.id_evento = opev.id_evento
                                                    and iop.id_opcion = opev.id_opcion
                                                    and iop.estado_registro = 'A'
                                                    group by iop.id_evento, iop.id_participante) total,
                                                    (
                                                    select mov.id_participante, mov.id_evento, sum(valor) valor_abono
                                                    from movimientos mov, evento ev
                                                    where mov.id_evento = ev.id_evento
                                                    and mov.tipo_mov = 'AB'
                                                    group by mov.id_participante, mov.id_evento) ab,
                                                    (
                                                    select mov.id_participante, mov.id_evento, sum(valor) valor_cargo
                                                    from movimientos mov, evento ev
                                                    where mov.id_evento = ev.id_evento
                                                    and mov.tipo_mov = 'CR'
                                                    group by mov.id_participante, mov.id_evento) cr
                                                    where principal.id_evento = total.id_evento
                                                    and principal.id_participante = total.id_participante
                                                    and  principal.id_evento = ab.id_evento(+)
                                                    and principal.id_participante = ab.id_participante(+)
                                                    and  principal.id_evento = cr.id_evento(+)
                                                    and principal.id_participante = cr.id_participante(+)
                                                    and principal.id_evento = :id_evento
                                                    order by principal.nombres,principal.id_evento";

        private const string _saldosDiariosDetallado = @"select id_evento, nombre_evento, tipo_pago, to_char(fecha,'dd/mm/yyyy') fecha_movimiento, total_abonos, total_cargos, total
                                                            from(
                                                            select gene.id_evento, gene.nombre_evento, gene.descripcion tipo_pago, gene.fecha, ab.total_abonos, cr.total_cargos,
                                                            nvl(ab.total_abonos,0)-nvl(cr.total_cargos,0) total
                                                            from(
                                                            select distinct(trunc(mov.fecha_movimiento)) fecha, mov.id_evento, ev.nombre_evento, tp.descripcion, mov.tipo_pago
                                                            from movimientos mov, evento ev, tipo_pago tp
                                                            where mov.id_evento = ev.id_evento
                                                            and mov.tipo_pago = tp.tipo_pago)gene,
                                                            (
                                                            select id_evento, tipo_pago, trunc(fecha_movimiento) fecha, sum(valor) total_abonos
                                                            from movimientos
                                                            where tipo_mov = 'AB'
                                                            group by id_evento, tipo_pago, trunc(fecha_movimiento))ab,
                                                            (
                                                            select id_evento, tipo_pago, trunc(fecha_movimiento) fecha, sum(valor) total_cargos
                                                            from movimientos
                                                            where tipo_mov = 'CR'
                                                            group by id_evento, tipo_pago, trunc(fecha_movimiento)) cr
                                                            where gene.id_evento = ab.id_evento(+)
                                                            and gene.tipo_pago = ab.tipo_pago(+)
                                                            and gene.fecha = ab.fecha(+)
                                                            and gene.id_evento = cr.id_evento(+)
                                                            and gene.tipo_pago = cr.tipo_pago(+)
                                                            and gene.fecha = cr.fecha(+)
                                                            and gene.id_evento = :id_evento
                                                            order by trunc(gene.fecha) desc)";

        private const string _opcionesEventoInscritos = @"select ins.id_evento, ev.nombre_evento, ins.id_participante, pa.nombre||' '||pa.apellido nombre_participante, 
                                                            iop.id_opcion, oev.descripcion descripcion_opcion, oev.precio,'X' marca 
                                                            from inscripcion ins, inscripcion_opcion iop, participante pa, evento ev, opcion_evento oev
                                                            where ins.id_participante = pa.id_participante
                                                            and ins.id_evento = ev.id_evento
                                                            and ins.id_participante = iop.id_participante
                                                            and ins.id_evento = iop.id_evento
                                                            and ins.id_evento = oev.id_evento
                                                            and iop.id_opcion = oev.id_opcion
                                                            and pa.estado_registro = 'A'
                                                            and ins.estado_registro = 'A'
                                                            and iop.estado_registro = 'A'
                                                            and ins.id_evento = :evento
                                                            order by ins.id_evento, nombre_participante, iop.id_opcion";

        private const string _reporteGeneralSaldoOpciones = @"select principal.id_evento,
                                                            principal.id_participante, 
                                                            principal.nombre_evento, 
                                                            principal.telefono,
                                                            principal.talla,
                                                            bupa.no_bus bus,
                                                            principal.nombres nombre_participante,
                                                            principal.genero,
                                                            to_char(principal.fecha_inscripcion,'dd/mm/yyyy hh24:mi:ss')fecha_inscripcion,
                                                            to_char(ultmov.ultimo_pago,'dd/mm/yyyy hh24:mi:ss') ultimo_pago,
                                                            total.total_evento_participante monto_total_evento,
                                                            nvl(ab.valor_abono,0) - nvl(cr.valor_cargo,0) monto_abonado, 
                                                            total.total_evento_participante - (nvl(ab.valor_abono,0) - nvl(cr.valor_cargo,0)) saldo_pendiente,
                                                            opc.descripcion_opcion, 
                                                            marca
                                                            from
                                                            ---------------------------------------SUB CONSULTA - DATOS DE EVENTO, INSCRIPCION Y PARTICIPANTE----------------------------------
                                                            (
                                                            select ins.id_evento, ins.id_participante, ins.fecha_inscripcion, pa.telefono, pa.talla, pa.genero, ev.nombre_evento, pa.nombre||' '||pa.apellido nombres
                                                            from inscripcion ins, participante pa, evento ev
                                                            where ins.id_participante = pa.id_participante
                                                            and ins.id_evento = ev.id_evento
                                                            and pa.estado_registro = 'A'
                                                            and ins.estado_registro = 'A'
                                                            )principal,
                                                            ---------------------------------------SUB CONSULTA - EL TOTAL QUE TIENE QUE PAGAR EL PARTICIPANTE----------------------------------
                                                            (
                                                            select iop.id_evento, iop.id_participante, sum(opev.precio)total_evento_participante
                                                            from inscripcion_opcion iop, opcion_evento opev
                                                            where iop.id_evento = opev.id_evento
                                                            and iop.id_opcion = opev.id_opcion
                                                            and iop.estado_registro = 'A'
                                                            group by iop.id_evento, iop.id_participante) total,
                                                            ---------------------------------------SUB CONSULTA - EL TOTAL DE ABONOS DEL PARTICIPANTE----------------------------------
                                                            (
                                                            select mov.id_participante, mov.id_evento, sum(valor) valor_abono
                                                            from movimientos mov, evento ev
                                                            where mov.id_evento = ev.id_evento
                                                            and mov.tipo_mov = 'AB'
                                                            group by mov.id_participante, mov.id_evento) ab,
                                                            ---------------------------------------SUB CONSULTA - EL TOTAL DE CARGOS DEL PARTICIPANTE----------------------------------
                                                            (
                                                            select mov.id_participante, mov.id_evento, sum(valor) valor_cargo
                                                            from movimientos mov, evento ev
                                                            where mov.id_evento = ev.id_evento
                                                            and mov.tipo_mov = 'CR'
                                                            group by mov.id_participante, mov.id_evento) cr,
                                                            ---------------------------------------SUB CONSULTA - OPCIONES DEL PARTICIPANTE----------------------------------
                                                            (
                                                            select ins.id_evento, ev.nombre_evento, ins.id_participante, pa.nombre||' '||pa.apellido nombre_participante, 
                                                            iop.id_opcion, oev.descripcion descripcion_opcion, oev.precio,'X' marca
                                                            from inscripcion ins, inscripcion_opcion iop, participante pa, evento ev, opcion_evento oev
                                                            where ins.id_participante = pa.id_participante
                                                            and ins.id_evento = ev.id_evento
                                                            and ins.id_participante = iop.id_participante
                                                            and ins.id_evento = iop.id_evento
                                                            and ins.id_evento = oev.id_evento
                                                            and iop.id_opcion = oev.id_opcion
                                                            and pa.estado_registro = 'A'
                                                            and ins.estado_registro = 'A'
                                                            and iop.estado_registro = 'A'
                                                            )opc,
                                                            ---------------------------------------SUB CONSULTA - NUMERO DE BUS SI EL PARTICIPANTE LO CONTRATO----------------------------------
                                                            (
                                                            select ins.id_evento, ins.id_participante, bins.no_bus
                                                            from inscripcion ins, inscripcion_opcion iop, bus_inscripcion bins, opcion_evento ope
                                                            where ins.id_evento = iop.id_evento
                                                            and ins.id_participante = iop.id_participante
                                                            and ins.id_evento = bins.id_evento(+)
                                                            and ins.id_participante = bins.id_participante(+)
                                                            and iop.id_evento = ope.id_evento
                                                            and iop.id_opcion = ope.id_opcion
                                                            and iop.estado_registro = 'A'
                                                            and ins.estado_registro = 'A'
                                                            and ope.es_transporte = 'S'
                                                            )bupa,
                                                            ---------------------------------------SUB CONSULTA - ULTIMO PAGO QUE REALIZO EL PARTICIPANTE----------------------------------
                                                            (
                                                            select id_participante, id_evento, max(fecha_movimiento) ultimo_pago
                                                            from movimientos
                                                            where tipo_mov = 'AB'
                                                            group by id_participante, id_evento
                                                            )ultmov
                                                            ---------------------------------------------------------------------------------------------------------------------------------------
                                                            where principal.id_evento = total.id_evento
                                                            and principal.id_participante = total.id_participante
                                                            and  principal.id_evento = ab.id_evento(+)
                                                            and principal.id_participante = ab.id_participante(+)
                                                            and  principal.id_evento = cr.id_evento(+)
                                                            and principal.id_participante = cr.id_participante(+)
                                                            and principal.id_evento = opc.id_evento
                                                            and principal.id_participante = opc.id_participante
                                                            and principal.id_evento = bupa.id_evento(+)
                                                            and principal.id_participante = bupa.id_participante(+)
                                                            and principal.id_participante = ultmov.id_participante(+)
                                                            and principal.id_evento = ultmov.id_evento(+)
                                                            and principal.id_evento = :id_evento
                                                            order by bupa.no_bus, principal.fecha_inscripcion, ultmov.ultimo_pago";


//        private const string _reporteGeneralSaldoOpciones = @"select principal.id_evento,principal.id_participante, principal.nombre_evento, principal.nombres nombre_participante, total.total_evento_participante monto_total_evento,
//                                                                nvl(ab.valor_abono,0) - nvl(cr.valor_cargo,0) monto_abonado, total.total_evento_participante - (nvl(ab.valor_abono,0) - nvl(cr.valor_cargo,0)) saldo_pendiente,
//                                                                opc.descripcion_opcion, marca
//                                                                from(
//                                                                select ins.id_evento, ins.id_participante, ev.nombre_evento, pa.nombre||' '||pa.apellido nombres
//                                                                from inscripcion ins, participante pa, evento ev
//                                                                where ins.id_participante = pa.id_participante
//                                                                and ins.id_evento = ev.id_evento
//                                                                and pa.estado_registro = 'A'
//                                                                and ins.estado_registro = 'A'
//                                                                )principal,
//                                                                (
//                                                                select iop.id_evento, iop.id_participante, sum(opev.precio)total_evento_participante
//                                                                from inscripcion_opcion iop, opcion_evento opev
//                                                                where iop.id_evento = opev.id_evento
//                                                                and iop.id_opcion = opev.id_opcion
//                                                                and iop.estado_registro = 'A'
//                                                                group by iop.id_evento, iop.id_participante) total,
//                                                                (
//                                                                select mov.id_participante, mov.id_evento, sum(valor) valor_abono
//                                                                from movimientos mov, evento ev
//                                                                where mov.id_evento = ev.id_evento
//                                                                and mov.tipo_mov = 'AB'
//                                                                group by mov.id_participante, mov.id_evento) ab,
//                                                                (
//                                                                select mov.id_participante, mov.id_evento, sum(valor) valor_cargo
//                                                                from movimientos mov, evento ev
//                                                                where mov.id_evento = ev.id_evento
//                                                                and mov.tipo_mov = 'CR'
//                                                                group by mov.id_participante, mov.id_evento) cr,
//                                                                (
//                                                                select ins.id_evento, ev.nombre_evento, ins.id_participante, pa.nombre||' '||pa.apellido nombre_participante, 
//                                                                iop.id_opcion, oev.descripcion descripcion_opcion, oev.precio,'X' marca 
//                                                                from inscripcion ins, inscripcion_opcion iop, participante pa, evento ev, opcion_evento oev
//                                                                where ins.id_participante = pa.id_participante
//                                                                and ins.id_evento = ev.id_evento
//                                                                and ins.id_participante = iop.id_participante
//                                                                and ins.id_evento = iop.id_evento
//                                                                and ins.id_evento = oev.id_evento
//                                                                and iop.id_opcion = oev.id_opcion
//                                                                and pa.estado_registro = 'A'
//                                                                and ins.estado_registro = 'A'
//                                                                and iop.estado_registro = 'A'
//                                                                )opc
//                                                                where principal.id_evento = total.id_evento
//                                                                and principal.id_participante = total.id_participante
//                                                                and  principal.id_evento = ab.id_evento(+)
//                                                                and principal.id_participante = ab.id_participante(+)
//                                                                and  principal.id_evento = cr.id_evento(+)
//                                                                and principal.id_participante = cr.id_participante(+)
//                                                                and principal.id_evento = opc.id_evento
//                                                                and principal.id_participante = opc.id_participante
//                                                                and principal.id_evento = :id_evento
//                                                                order by principal.nombres,principal.id_evento";

        #endregion

        #region Metodos Publicos

        /// <summary>
        /// Metodo para obtener la informacion del Reporte del Recibo
        /// </summary>
        /// <returns></returns>
        public List<ReciboMovimiento> ObtenerDatosRecibo()
        {
            List<ReciboMovimiento> result = new List<ReciboMovimiento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder strSQL = new StringBuilder();

                    strSQL.Append(" select rec.numero_recibo, mov.id_movimiento, mov.id_participante, mov.id_evento, rdet.opcion, rdet.precio, mov.valor, ev.nombre_evento, ");
                    strSQL.Append(" pa.nombre||' '||pa.apellido nombre_participante, to_char(mov.fecha_movimiento,'dd/mm/yyyy') fecha_movimiento, rec.monto_total_evento, ");
                    strSQL.Append(" rec.monto_abonado, rec.saldo_pendiente, nvl(to_char(busasignado.no_bus),'NO APLICA') no_bus, nvl(busasignado.hora_salida,'-----') hora_salida, ");
                    strSQL.Append(" nvl(nore.nota,'No Hay Notas para este Evento de Momento') nota ");
                    strSQL.Append(" from participante pa, evento ev, movimientos mov, recibo rec, recibo_detalle rdet, notas_evento_recibo nore, ");
                    strSQL.Append(" ( ");
                    strSQL.Append(" select bin.id_participante, bin.id_evento, bev.no_bus, bev.hora_salida ");
                    strSQL.Append(" from bus_inscripcion bin, bus_evento bev ");
                    strSQL.Append(" where bin.id_bus = bev.id_bus ");
                    strSQL.Append(" and bin.id_evento = bev.id_evento ");
                    strSQL.Append(" ) busasignado ");
                    strSQL.Append(" where mov.id_participante = pa.id_participante ");
                    strSQL.Append(" and mov.id_evento = ev.id_evento ");
                    strSQL.Append(" and mov.id_movimiento = rec.id_movimiento ");
                    strSQL.Append(" and mov.id_movimiento = rdet.id_movimiento ");
                    strSQL.Append(" and mov.id_evento = nore.id_evento(+) ");
                    strSQL.Append(" and mov.id_evento = busasignado.id_evento(+) ");
                    strSQL.Append(" and mov.id_participante = busasignado.id_participante(+) ");
                    strSQL.Append(" and mov.id_movimiento = :id_movimiento ");
                    strSQL.Append(" order by rdet.id_opcion ");


                    var list = db.Database.SqlQuery<ReciboMovimiento>(strSQL.ToString(), new object[] { id_movimiento
                                                                                                        }).ToList<ReciboMovimiento>();

                    result = list;
                    if (list == null)
                    {
                        result = new List<ReciboMovimiento>();
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return result;
        }

        /// <summary>
        /// Metodo para obtener la informacion del Listado por Evento
        /// </summary>
        /// <returns></returns>
        public Response<List<ReciboMovimiento>> listadoPorEvento()
        {
            Response<List<ReciboMovimiento>> result = new Response<List<ReciboMovimiento>>();
            result.code = 1;
            result.message = "Ocurrio un Error en base de datos al tratar de obtener el listado";
            result.data = new List<ReciboMovimiento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder sqlStr = new StringBuilder();
                    sqlStr.Append(_listadoporEvento);

                    var list = db.Database.SqlQuery<ReciboMovimiento>(sqlStr.ToString(), new object[] { id_evento }).ToList<ReciboMovimiento>();

                    if (list.Count == 0)
                    {
                        result.code = -1;
                        result.message = "No hay registros para este Evento";
                        result.data = new List<ReciboMovimiento>();
                        return result;
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            result.data.Add(item);
                        }
                    }
                }
                result.code = 0;
                result.message = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.code = 1;
                result.message = "Ocurrio una Excepcion al tratar de obtener el listado: " + ex.ToString();
                return result;
            }            
        }

        /// <summary>
        /// Metodo que devuelve el listado de Abonos y Cargos por Fecha Cobrado
        /// </summary>
        /// <returns></returns>
        public Response<List<ReciboMovimiento>> saldosDiariosDetallado()
        {
            Response<List<ReciboMovimiento>> result = new Response<List<ReciboMovimiento>>();
            result.code = 1;
            result.message = "Ocurrio un Error en base de datos al tratar de obtener el listado";
            result.data = new List<ReciboMovimiento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder sqlStr = new StringBuilder();
                    sqlStr.Append(_saldosDiariosDetallado);

                    var list = db.Database.SqlQuery<ReciboMovimiento>(sqlStr.ToString(), new object[] { id_evento }).ToList<ReciboMovimiento>();

                    if (list.Count == 0)
                    {
                        result.code = -1;
                        result.message = "No hay registros para este Evento";
                        result.data = new List<ReciboMovimiento>();
                        return result;
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            result.data.Add(item);
                        }
                    }
                }
                result.code = 0;
                result.message = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.code = 1;
                result.message = "Ocurrio una Excepcion al tratar de obtener el listado: " + ex.ToString();
                return result;
            }
        }

        /// <summary>
        /// Metodo que devuelve el listado de participantes y sus Opciones por Evento
        /// </summary>
        /// <returns></returns>
        public Response<List<ReciboMovimiento>> OpcionesEventosInscritos()
        {
            Response<List<ReciboMovimiento>> result = new Response<List<ReciboMovimiento>>();
            result.code = 1;
            result.message = "Ocurrio un Error en base de datos al tratar de obtener el listado";
            result.data = new List<ReciboMovimiento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder sqlStr = new StringBuilder();
                    sqlStr.Append(_opcionesEventoInscritos);

                    var list = db.Database.SqlQuery<ReciboMovimiento>(sqlStr.ToString(), new object[] { id_evento }).ToList<ReciboMovimiento>();

                    if (list.Count == 0)
                    {
                        result.code = -1;
                        result.message = "No hay registros para este Evento";
                        result.data = new List<ReciboMovimiento>();
                        return result;
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            result.data.Add(item);
                        }
                    }
                }
                result.code = 0;
                result.message = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.code = 1;
                result.message = "Ocurrio una Excepcion al tratar de obtener el listado: " + ex.ToString();
                return result;
            }
        }

        /// <summary>
        /// Metodo que devuelve el listado General de Saldos y Opciones de Participantes por Evento
        /// </summary>
        /// <returns></returns>
        public Response<List<ReciboMovimiento>> reporteGeneralSaldoOpciones()
        {
            Response<List<ReciboMovimiento>> result = new Response<List<ReciboMovimiento>>();
            result.code = 1;
            result.message = "Ocurrio un Error en base de datos al tratar de obtener el listado";
            result.data = new List<ReciboMovimiento>();

            try
            {
                using (var db = new EntitiesEvento())
                {
                    StringBuilder sqlStr = new StringBuilder();
                    sqlStr.Append(_reporteGeneralSaldoOpciones);

                    var list = db.Database.SqlQuery<ReciboMovimiento>(sqlStr.ToString(), new object[] { id_evento }).ToList<ReciboMovimiento>();

                    if (list.Count == 0)
                    {
                        result.code = -1;
                        result.message = "No hay registros para este Evento";
                        result.data = new List<ReciboMovimiento>();
                        return result;
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            result.data.Add(item);
                        }
                    }
                }
                result.code = 0;
                result.message = "Ok";
                return result;
            }
            catch (Exception ex)
            {
                result.code = 1;
                result.message = "Ocurrio una Excepcion al tratar de obtener el listado: " + ex.ToString();
                return result;
            }
        }
        #endregion
    }
}