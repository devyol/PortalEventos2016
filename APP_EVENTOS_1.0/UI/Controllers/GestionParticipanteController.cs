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
    public class GestionParticipanteController : ApiController
    {
        [HttpPost]
        public Response<List<Participante>> ListaParticipantes()
        {
            Response<List<Participante>> obj = new Response<List<Participante>>();
            Participantes lis = new Participantes();
            return obj = lis.ListaParticipantes();
        }

        [HttpPost]
        public Response<Participante> ListarParticipante([FromBody] Participante pa)
        {
            Response<Participante> obj = new Response<Participante>();
            Participantes li = new Participantes();
            return obj = li.ListarParticipante(pa.id);
        }

        [HttpPost]
        public Response<Participante> GuardarParticipante([FromBody] Participante pa)
        {
            Response<Participante> obj = new Response<Participante>();
            Participantes tra = new Participantes();

            if (pa.operacion == "Nuevo")
            {
                return obj = tra.RegistrarParticipante(pa);
            }
            else
            {
                return obj = tra.ActualizarParticipante(pa);
            }
        }
    }
}
