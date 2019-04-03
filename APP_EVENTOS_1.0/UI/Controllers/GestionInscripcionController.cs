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
    public class GestionInscripcionController : ApiController
    {
        [HttpPost]
        public Response<List<Evento>> EventosParaInscripcion([FromBody] Participante arg)
        {
            Response<List<Evento>> obj = new Response<List<Evento>>();
            Inscripciones li = new Inscripciones();
            return obj = li.EventosporInscribir(arg);
        }

        [HttpPost]
        public Response<Inscripcion> InscribirParticipante([FromBody] Inscripcion arg)
        {
            Response<Inscripcion> obj = new Response<Inscripcion>();
            Inscripciones Ins = new Inscripciones();
            return obj = Ins.InsertarInscripcion(arg);
        }

        [HttpPost]
        public Response<List<Inscripcion>> EventosInscritos([FromBody]Participante arg)
        {
            Response<List<Inscripcion>> obj = new Response<List<Inscripcion>>();
            Inscripciones li = new Inscripciones();
            return obj = li.EventosInscritos(arg);
        }

        [HttpPost]
        public Response<List<InscripcionOpcion>> OpcionesDelInscrito([FromBody] Inscripcion arg)
        {
            Response<List<InscripcionOpcion>> obj = new Response<List<InscripcionOpcion>>();
            Inscripciones liOpIns = new Inscripciones();
            return obj = liOpIns.OpcionesInscrito(arg);
        }

        [HttpPost]
        public Response<InscripcionOpcion> ModificarOpcionInscripcion([FromBody] InscripcionOpcion arg)
        {
            Response<InscripcionOpcion> obj = new Response<InscripcionOpcion>();
            Inscripciones act = new Inscripciones();
            return obj = act.ActualizarOpcionInscripcion(arg);
        }
    }
}
