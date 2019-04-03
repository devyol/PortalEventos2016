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
    public class ReportesEventoController : ApiController
    {

        [HttpPost]
        public Response<List<ReciboMovimiento>> ListarInscritosEvento([FromBody] decimal idevento)
        {
            Response<List<ReciboMovimiento>> resp = new Response<List<ReciboMovimiento>>();
            ReciboMovimiento obj = new ReciboMovimiento() { id_evento = idevento};
            return resp = obj.listadoPorEvento();
        }

        [HttpPost]
        public Response<List<ReciboMovimiento>> ListarSaldosDiarios([FromBody] decimal idevento)
        {
            Response<List<ReciboMovimiento>> resp = new Response<List<ReciboMovimiento>>();
            ReciboMovimiento obj = new ReciboMovimiento() { id_evento = idevento };
            return resp = obj.saldosDiariosDetallado();
        }

        [HttpPost]
        public Response<List<ReciboMovimiento>> ListarOpcionesInscritos([FromBody] decimal idevento)
        {
            Response<List<ReciboMovimiento>> resp = new Response<List<ReciboMovimiento>>();
            ReciboMovimiento obj = new ReciboMovimiento() { id_evento = idevento };
            return resp = obj.OpcionesEventosInscritos();
        }

    }
}
