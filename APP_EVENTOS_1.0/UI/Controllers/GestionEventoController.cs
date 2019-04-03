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
    public class GestionEventoController : ApiController
    {
        [HttpPost]
        public Response<List<Evento>> ListarEventos()
        {
            Response<List<Evento>> obj = new Response<List<Evento>>();
            GestionEvento list = new GestionEvento();
            return obj = list.ListEventos();
        }

        [HttpPost]
        [ActionName("ListarEvento")]
        public Response<Evento> ListarEvento([FromBody] Evento ev)
        {
            Response<Evento> obj = new Response<Evento>();
            GestionEvento list = new GestionEvento();
            return obj = list.ListEvento(ev);
        }

        [HttpPost]
        [ActionName("GuardarEvento")]
        public Response<Evento> GuardarEvento([FromBody] Evento ev)
        {
            Response<Evento> obj = new Response<Evento>();
            GestionEvento transaccion = new GestionEvento();

            if (ev.operacion=="Nuevo")
            {
                return obj = transaccion.InsertEvento(ev);
            }
            else
            {
                return obj = transaccion.UpdateEvento(ev);
            }
        }
    }
}