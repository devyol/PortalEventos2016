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
    public class GestionBusInscripcionController : ApiController
    {
        [HttpPost]
        public Response<List<Evento>> EventosConBus()
        {
            Response<List<Evento>> obj = new Response<List<Evento>>();
            GestionBusInscripcion eventos = new GestionBusInscripcion();
            return obj = eventos.EventosConBus();
        }

        [HttpPost]
        public Response<List<Participante>> ListarParticipantes([FromBody]decimal evento)
        {
            Response<List<Participante>> obj = new Response<List<Participante>>();
            GestionBusInscripcion listaPar = new GestionBusInscripcion();
            return obj = listaPar.ParticipantesPorEvento(evento);
        }

        [HttpPost]
        public Response<Participante> ParticipanteConTransporte([FromBody]Participante arg)
        {
            Response<Participante> obj = new Response<Participante>();
            GestionBusInscripcion list = new GestionBusInscripcion();
            return obj = list.ParticipanteConTransporte(arg);
        }

        [HttpPost]
        public Response<List<Bus>> BusesEvento([FromBody]decimal evento)
        {
            Response<List<Bus>> obj = new Response<List<Bus>>();
            GestionBusInscripcion buses = new GestionBusInscripcion();
            return obj = buses.BusesEvento(evento);
        }

        [HttpPost]
        public Response<List<Bus>> BusesDisponibles([FromBody] Participante arg)
        {
            Response<List<Bus>> obj = new Response<List<Bus>>();
            GestionBusInscripcion listBus = new GestionBusInscripcion();
            return obj = listBus.BusesDisponiblesAsignar(arg);
        }

        [HttpPost]
        public Response<List<BusInscripcion>> BusAsignado([FromBody] Participante arg)
        {
            Response<List<BusInscripcion>> obj = new Response<List<BusInscripcion>>();
            GestionBusInscripcion bus = new GestionBusInscripcion();
            return obj = bus.BusAsignado(arg);
        }

        [HttpPost]
        public Response<BusInscripcion> BusAsignadoDataUnico([FromBody] Participante arg)
        {
            Response<BusInscripcion> obj = new Response<BusInscripcion>();
            GestionBusInscripcion busUnico = new GestionBusInscripcion();
            return obj = busUnico.BusAsignadoDatoUnico(arg);
        }

        [HttpPost]
        public Response<BusInscripcion> AsignarBusParticipante([FromBody] BusInscripcion arg)
        {
            Response<BusInscripcion> obj = new Response<BusInscripcion>();
            GestionBusInscripcion guardar = new GestionBusInscripcion();
            return obj = guardar.AsignarBusParticipante(arg);
        }

        [HttpPost]
        public Response<List<Bus>> BusesDisponiblesModificar([FromBody] Participante arg)
        {
            Response<List<Bus>> obj = new Response<List<Bus>>();
            GestionBusInscripcion listaBus = new GestionBusInscripcion();
            return obj = listaBus.BusesDisponiblesModificar(arg);
        }

        [HttpPost]
        public Response<BusInscripcion> ModificarBusParticipante([FromBody] BusInscripcion arg)
        {
            Response<BusInscripcion> obj = new Response<BusInscripcion>();
            GestionBusInscripcion actualizar = new GestionBusInscripcion();
            return obj = actualizar.ModificarBus(arg);
        }        
        
    }
}
