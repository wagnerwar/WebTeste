using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reports;
using Reports.DataSets;
using System.IO;
using System.Text.RegularExpressions;
using TesteWeb.Models;
namespace TesteWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

 
        [HttpGet]
        public ActionResult GerarExcel()
        {
            try
            {
                String mimeType = String.Empty;
                byte[] relatorio = this.GerarRelatorioExcel(out mimeType);
                Session["arquivo"] = relatorio;
                Session["tipo"] = mimeType;
                return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { ok = false });
            }
        }
        public ActionResult Imprimir()
        {
            try
            {
                if(Session["arquivo"] != null)
                {
                    byte[] relatorio = (byte[])Session["arquivo"];
                    String mimeType = (String)Session["tipo"];
                    var result = (FileContentResult)File(relatorio, mimeType);
                    string nomeRelatorio = string.Empty;
                    nomeRelatorio = "relatorio_TESTEs_" + DateTime.Now.ToString("ddMMyyyy") + ".xls";
                    result.FileDownloadName = nomeRelatorio;
                    return result;
                }
                else
                {
                    throw new Exception("Não há arquivo gerado");
                }                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private byte[] GerarRelatorioExcel(out String mimeType)
        {
            // Montando dataSet
            dsTabela dataSet = new Reports.DataSets.dsTabela();
            dsTabela.TabelaDataTable tabela = new dsTabela.TabelaDataTable();
            tabela.AddTabelaRow( "1", "teste");

            // Montando o relatório
            List<DataSetRelatorio> listaDataSet = new List<DataSetRelatorio>();
            Dictionary<string, object> reportParameters = new Dictionary<string, object>();
            listaDataSet.Add(new DataSetRelatorio { dataSetName = "dsTabela", dataTable = tabela });
            Report reportBytes = new Report(listaDataSet, reportParameters, Report.REP_TESTE, ReportType.Excel);
            byte[] retorno = reportBytes.Generate(out mimeType);
            return retorno;
        }
    }
}