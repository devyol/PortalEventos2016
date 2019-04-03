using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UI.Entidades;
using UI.Models;

namespace UI.Controllers
{
    public class GestionCobroController : ApiController
    {
        [HttpPost]
        public Response<List<SaldoEvento>> SaldosEventosInscritos(Participante arg)
        {
            Response<List<SaldoEvento>> obj = new Response<List<SaldoEvento>>();
            GestionCobro li = new GestionCobro();
            return obj = li.SaldosEventos(arg);
        }

        [HttpPost]
        public Response<SaldoEvento> SaldoEventoInscrito(SaldoEvento arg)
        {
            Response<SaldoEvento> obj = new Response<SaldoEvento>();
            GestionCobro li = new GestionCobro();
            return obj = li.SaldoEvento(arg);
        }

        [HttpPost]
        public Response<Movimiento> AbonarSaldo(Movimiento arg)
        {
            Response<Movimiento> obj = new Response<Movimiento>();
            GestionCobro insert = new GestionCobro();
            return obj = insert.AbonarSaldo(arg);
        }

        [HttpPost]
        public Response<List<Movimiento>> ListadoMovimientosEventoInscrito(SaldoEvento arg)
        {
            Response<List<Movimiento>> obj = new Response<List<Movimiento>>();
            GestionCobro li = new GestionCobro();
            return obj = li.ListarMovimientosEvento(arg);
        }
    }
}