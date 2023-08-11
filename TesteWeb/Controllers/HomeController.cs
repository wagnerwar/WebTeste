﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reports;
using Reports.DataSets;
using System.IO;
using System.Text.RegularExpressions;
using TesteWeb.Models;
using Testes;
using System.Web.Helpers;
using TesteWeb.Libs;
namespace TesteWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var teste = new ProdamGeoLougradouroService().ConsultarLogradouroPorCep("05773020");
            return View();
        }
        public ActionResult Grafico()
        {
            return View();
        }

        public JsonResult GerarGrafico()
        {
            RetornoGraficoViewModel retorno = new RetornoGraficoViewModel();
            try
            {
                GeradorGrafico gerador = new GeradorGrafico();
                var imagem = gerador.GerarImagemTotalizadorGrid("100", 120, 60);
                retorno.Base64 = imagem;
            }catch(Exception ex)
            {
                throw ex;
            }
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RedimensionarGrafico()
        {
            RetornoGraficoViewModel retorno = new RetornoGraficoViewModel();
            try
            {
                GeradorGrafico gerador = new GeradorGrafico();
                var imagem = gerador.RedimensionarImagemTotalizadorGrid("100", 120, 60);
                retorno.Base64 = imagem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AgruparGrafico()
        {
            RetornoGraficoViewModel retorno = new RetornoGraficoViewModel();
            try
            {
                GeradorGrafico gerador = new GeradorGrafico();
                var imagem = gerador.AgruparImagemTotalizadorGrid("100", 120, 60, null);
                retorno.Base64 = imagem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AgruparBlocoGrafico()
        {
            RetornoGraficoViewModel retorno = new RetornoGraficoViewModel();
            try
            {
                GeradorGrafico gerador = new GeradorGrafico();
                var imagem = gerador.AgruparImagemTotalizadorBloco("100", 120, 60, null);
                retorno.Base64 = imagem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(retorno, JsonRequestBehavior.AllowGet);
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
        [HttpGet]
        public ActionResult GerarPDF()
        {
            try
            {
                String mimeType = String.Empty;
                byte[] relatorio = this.GerarRelatorioPDF(out mimeType);
                Session["arquivo"] = relatorio;
                Session["tipo"] = mimeType;
                return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
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
        public ActionResult ImprimirPDF()
        {
            try
            {
                if (Session["arquivo"] != null)
                {
                    byte[] relatorio = (byte[])Session["arquivo"];
                    String mimeType = (String)Session["tipo"];
                    var result = (FileContentResult)File(relatorio, mimeType);
                    string nomeRelatorio = string.Empty;
                    nomeRelatorio = "relatorio_TESTEs_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    result.FileDownloadName = nomeRelatorio;
                    return result;
                }
                else
                {
                    throw new Exception("Não há arquivo gerado");
                }
            }
            catch (Exception ex)
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
            reportParameters.Add("ExibirRotulo", false);
            reportParameters.Add("ExibirColuna", false);
            listaDataSet.Add(new DataSetRelatorio { dataSetName = "dsTabela", dataTable = tabela });
            Report reportBytes = new Report(listaDataSet, reportParameters, Report.REP_TESTE, ReportType.Excel);
            byte[] retorno = reportBytes.Generate(out mimeType);
            return retorno;
        }
        private byte[] GerarRelatorioPDF(out String mimeType)
        {
            // Montando dataSet
            dsTabela dataSet = new Reports.DataSets.dsTabela();
            dsTabela.TabelaDataTable tabela = new dsTabela.TabelaDataTable();
            tabela.AddTabelaRow("1", "teste");
            // Montando o relatório
            List<DataSetRelatorio> listaDataSet = new List<DataSetRelatorio>();
            Dictionary<string, object> reportParameters = new Dictionary<string, object>();
            reportParameters.Add("ExibirRotulo", false);
            reportParameters.Add("ExibirColuna", false);
            listaDataSet.Add(new DataSetRelatorio { dataSetName = "dsTabela", dataTable = tabela });
            Report reportBytes = new Report(listaDataSet, reportParameters, Report.REP_TESTE, ReportType.PDF);
            byte[] retorno = reportBytes.Generate(out mimeType);
            return retorno;
        }
    }
}