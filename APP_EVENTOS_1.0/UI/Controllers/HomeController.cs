using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Entidades;
using UI.Models;
using Microsoft.Reporting.WebForms;
using System.IO;


namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public FileResult obtenerRecibo(decimal idMov)
        {
            List<ReciboMovimiento> Servicios = new List<ReciboMovimiento>();
            ReciboMovimiento objServicio = new ReciboMovimiento() { id_movimiento = idMov };
            Servicios = objServicio.ObtenerDatosRecibo();


            ReportViewer rv = new ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = Server.MapPath("~/Reporte/RptRecibo.rdlc");
            rv.LocalReport.DataSources.Clear();
            ReportDataSource dsEncabezado = new ReportDataSource("dtsRecibo", Servicios);
            rv.LocalReport.DataSources.Add(dsEncabezado);
            rv.LocalReport.Refresh();

            byte[] streamBytes = null;
            string mimeType = "";
            string enconding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;

            streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out enconding, out filenameExtension, out streamids, out warnings);
            return File(streamBytes, mimeType);
        }

        public FileResult listaInscritosEventoPdf(decimal idEvento)
        {
            Response<List<ReciboMovimiento>> Servicios = new Response<List<ReciboMovimiento>>();             
            ReciboMovimiento objServicio = new ReciboMovimiento() { id_evento = idEvento };
            Servicios.data = objServicio.listadoPorEvento().data;


            ReportViewer rv = new ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = Server.MapPath("~/Reporte/Listado.rdlc");
            rv.LocalReport.DataSources.Clear();
            ReportDataSource dsEncabezado = new ReportDataSource("dts_Inscritos", Servicios.data);
            rv.LocalReport.DataSources.Add(dsEncabezado);
            rv.LocalReport.Refresh();

            byte[] streamBytes = null;
            string mimeType = "";
            string enconding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;

            streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out enconding, out filenameExtension, out streamids, out warnings);
            return File(streamBytes, mimeType);
        }

        public FileResult listaInscritosEventoExcel(decimal idEvento)
        {
            Response<List<ReciboMovimiento>> Servicios = new Response<List<ReciboMovimiento>>();
            ReciboMovimiento objServicio = new ReciboMovimiento() { id_evento = idEvento };
            Servicios.data = objServicio.listadoPorEvento().data;


            ReportViewer rv = new ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = Server.MapPath("~/Reporte/Listado.rdlc");
            rv.LocalReport.DataSources.Clear();
            ReportDataSource dsEncabezado = new ReportDataSource("dts_Inscritos", Servicios.data);
            rv.LocalReport.DataSources.Add(dsEncabezado);
            rv.LocalReport.Refresh();

            byte[] streamBytes = null;
            string mimeType = "";
            string enconding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;

            streamBytes = rv.LocalReport.Render("Excel", null, out mimeType, out enconding, out filenameExtension, out streamids, out warnings);
            return File(streamBytes, mimeType);
        }

        public FileResult opcionesInscritosEventoPdf(decimal idEvento)
        {
            Response<List<ReciboMovimiento>> Servicios = new Response<List<ReciboMovimiento>>();
            ReciboMovimiento objServicio = new ReciboMovimiento() { id_evento = idEvento };
            Servicios.data = objServicio.OpcionesEventosInscritos().data;


            ReportViewer rv = new ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = Server.MapPath("~/Reporte/RptOpcionesInscritos.rdlc");
            rv.LocalReport.DataSources.Clear();
            ReportDataSource dsEncabezado = new ReportDataSource("dts_OpcionesInscritos", Servicios.data);
            rv.LocalReport.DataSources.Add(dsEncabezado);
            rv.LocalReport.Refresh();

            byte[] streamBytes = null;
            string mimeType = "";
            string enconding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;

            streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out enconding, out filenameExtension, out streamids, out warnings);
            return File(streamBytes, mimeType);
        }

        public FileResult opcionesInscritosEventoExcel(decimal idEvento)
        {
            Response<List<ReciboMovimiento>> Servicios = new Response<List<ReciboMovimiento>>();
            ReciboMovimiento objServicio = new ReciboMovimiento() { id_evento = idEvento };
            Servicios.data = objServicio.OpcionesEventosInscritos().data;


            ReportViewer rv = new ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = Server.MapPath("~/Reporte/RptOpcionesInscritos.rdlc");
            rv.LocalReport.DataSources.Clear();
            ReportDataSource dsEncabezado = new ReportDataSource("dts_OpcionesInscritos", Servicios.data);
            rv.LocalReport.DataSources.Add(dsEncabezado);
            rv.LocalReport.Refresh();

            byte[] streamBytes = null;
            string mimeType = "";
            string enconding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;

            streamBytes = rv.LocalReport.Render("Excel", null, out mimeType, out enconding, out filenameExtension, out streamids, out warnings);
            return File(streamBytes, mimeType);
        }

        public FileResult reportaGeneralSaldosOpcionesPdf(decimal idEvento)
        {
            Response<List<ReciboMovimiento>> Servicios = new Response<List<ReciboMovimiento>>();
            ReciboMovimiento objServicio = new ReciboMovimiento() { id_evento = idEvento };
            Servicios.data = objServicio.reporteGeneralSaldoOpciones().data;


            ReportViewer rv = new ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = Server.MapPath("~/Reporte/RptSaldosOpciones.rdlc");
            rv.LocalReport.DataSources.Clear();
            ReportDataSource dsEncabezado = new ReportDataSource("dts_SaldosOpciones", Servicios.data);
            rv.LocalReport.DataSources.Add(dsEncabezado);
            rv.LocalReport.Refresh();

            byte[] streamBytes = null;
            string mimeType = "";
            string enconding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;

            streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out enconding, out filenameExtension, out streamids, out warnings);
            return File(streamBytes, mimeType);
        }

        public FileResult reportaGeneralSaldosOpcionesExcel(decimal idEvento)
        {
            Response<List<ReciboMovimiento>> Servicios = new Response<List<ReciboMovimiento>>();
            ReciboMovimiento objServicio = new ReciboMovimiento() { id_evento = idEvento };
            Servicios.data = objServicio.reporteGeneralSaldoOpciones().data;


            ReportViewer rv = new ReportViewer();
            rv.ProcessingMode = ProcessingMode.Local;
            rv.LocalReport.ReportPath = Server.MapPath("~/Reporte/RptSaldosOpciones.rdlc");
            rv.LocalReport.DataSources.Clear();
            ReportDataSource dsEncabezado = new ReportDataSource("dts_SaldosOpciones", Servicios.data);
            rv.LocalReport.DataSources.Add(dsEncabezado);
            rv.LocalReport.Refresh();

            byte[] streamBytes = null;
            string mimeType = "";
            string enconding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;

            streamBytes = rv.LocalReport.Render("Excel", null, out mimeType, out enconding, out filenameExtension, out streamids, out warnings);
            return File(streamBytes, mimeType);
        }

    }
}
