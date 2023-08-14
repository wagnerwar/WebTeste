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
                var imagem = gerador.GerarImagemTotalizadorComTituloBarra("100", 140, 60);
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
                var imagem = gerador.AgruparImagemTotalizadorBloco("100", 140, 60, null);
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
    }
}