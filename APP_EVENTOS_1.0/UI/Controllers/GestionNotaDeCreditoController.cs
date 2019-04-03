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
    public class GestionNotaDeCreditoController : ApiController
    {
        [HttpPost]
        public Response<List<Evento>> ListarEventosNota()
        {
            Response<List<Evento>> obj = new Response<List<Evento>>();
            GestionNotaCredito lista = new GestionNotaCredito();
            return obj = lista.ListaEventoActivos();
        }

        [HttpPost]
        public Response<List<Participante>> ListarParticipantesNota([FromBody] decimal arg)
        {
            Response<List<Participante>> obj = new Response<List<Participante>>();
            GestionNotaCredito lista = new GestionNotaCredito();
            return obj = lista.ListaParticipantes(arg);
        }

        [HttpPost]
        public Response<SaldoEvento> SaldoEventoNota([FromBody] Participante arg)
        {
            Response<SaldoEvento> obj = new Response<SaldoEvento>();
            GestionNotaCredito Saldo = new GestionNotaCredito();
            return obj = Saldo.SaldoEventoNota(arg);
        }

        [HttpPost]
        public Response<List<Movimiento>> ListarCargosNota([FromBody] Participante arg)
        {
            Response<List<Movimiento>> obj = new Response<List<Movimiento>>();
            GestionNotaCredito lista = new GestionNotaCredito();
            return obj = lista.ListarCargosNota(arg);
        }

        [HttpPost]
        public Response<Movimiento> GenerarNotaCredito([FromBody] Movimiento arg)
        {
            Response<Movimiento> obj = new Response<Movimiento>();
            GestionNotaCredito registrar = new GestionNotaCredito();
            return obj = registrar.GenerarNotaDeCredito(arg);
        }
        
    }
}
