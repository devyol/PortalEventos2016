using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UI.Models;
using UI.Entidades;

namespace UI.Controllers
{
    public class OpcionEventoController : ApiController
    {
        [HttpPost]
        public Response<List<Evento>> ListaEventoActivo()
        {
            Response<List<Evento>> obj = new Response<List<Evento>>();
            OpcionesEventos lista = new OpcionesEventos();
            return obj = lista.ListaEventosActivos();
        }

        [HttpPost]
        [ActionName("ListaOpcionesEvento")]
        public Response<List<OpcionEvento>> ListaOpcionesEvento([FromBody]decimal arg)
        {
            Response<List<OpcionEvento>> obj = new Response<List<OpcionEvento>>();
            OpcionesEventos listaOp = new OpcionesEventos();
            return obj = listaOp.ListarOpcionesEvento(arg);
        }

        [HttpPost]
        [ActionName("InformacionOpcionEvento")]
        public Response<OpcionEvento> InformacionOpcionEvento([FromBody] OpcionEvento arg)
        {
            Response<OpcionEvento> obj = new Response<OpcionEvento>();
            OpcionesEventos listaEvento = new OpcionesEventos();
            return obj = listaEvento.ListarOpcionEvento(arg.id);
        }

        [HttpPost]
        [ActionName("GuardarOpcionEvento")]
        public Response<OpcionEvento> GuardarOpcionEvento([FromBody] OpcionEvento arg)
        {
            Response<OpcionEvento> obj = new Response<OpcionEvento>();
            OpcionesEventos transaccion = new OpcionesEventos();

            if (arg.operacion == "Nuevo")
            {
                return obj = transaccion.InsertarOpcionEvento(arg);
            }
            else
            {
                return obj = transaccion.ActualizarOpcionEvento(arg);
            }
        }

        [HttpPost]
        [ActionName("EliminarOpcionEvento")]
        public Response<OpcionEvento> EliminarOpcionEvento([FromBody] OpcionEvento arg)
        {
            Response<OpcionEvento> obj = new Response<OpcionEvento>();
            OpcionesEventos elimina = new OpcionesEventos();
            return obj = elimina.EliminarOpcionEvento(arg);
        }
        
    }
}
