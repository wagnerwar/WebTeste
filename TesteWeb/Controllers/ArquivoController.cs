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
    public class ArquivoController : Controller
    {
        // GET: Arquivo
        public ActionResult Index()
        {
            return View();
        }
        // Envio de cada arquivo
        [HttpPost]
        public ActionResult EnvioArquivo(HttpPostedFileBase file, String nome)
        {
            try
            {
                //throw new Exception("Zaz");
                String caminhoGravacaoArquivos = @"C:\caminhoTeste\";
                String extensao = String.Empty;
                String mimeType = String.Empty;
                mimeType = file.ContentType;
                extensao = this.GetExtensionByMimeType(mimeType);
                String nomeArquivoGerado = this.GerarNomeArquivo(nome);
                String nomeArquivo = String.Format("{0}{1}.{2}", caminhoGravacaoArquivos, nomeArquivoGerado, extensao);
                // Obtendo bytes
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }
                // Escrevendo bytes no caminho especificado
                using (FileStream fileStream = new FileStream(nomeArquivo, FileMode.Create))
                {
                    for (int i = 0; i < fileData.Length; i++)
                    {
                        fileStream.WriteByte(fileData[i]);
                    }
                }
                return Json(new { msg = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
                //return Json(new { msg = "NOK" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult RemoverArquivo(String nome)
        {
            String caminhoGravacaoArquivos = @"C:\caminhoTeste\";
            String nomeArquivoParticipante = this.GerarNomeArquivo(nome); // Montar nome com base nas informações do participante
            String caminhoAPesquisar = String.Format(@"^{0}.*$", nomeArquivoParticipante);
            Regex exp = new Regex(caminhoAPesquisar);
            try
            {
                // Listar conteúdo do diretório
                String[] arquivos = Directory.GetFiles(caminhoGravacaoArquivos);
                foreach (var arq in arquivos)
                {
                    String nomeArquivo = arq.Replace(caminhoGravacaoArquivos, "");
                    bool encontrou = exp.IsMatch(nomeArquivo);
                    if (encontrou)
                    {
                        // remove o arquivo
                        String caminhoArquivoCompleto = String.Format("{0}{1}", caminhoGravacaoArquivos, nomeArquivo);
                        System.IO.File.Delete(caminhoArquivoCompleto);
                        break;
                    }
                }
                return Json(new { msg = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = "NOK" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ImprimirArquivo(String Nome)
        {
            try
            {
                String caminhoArquivo = @"C:\Users\p017432\Downloads\spinner.gif";
                string mimeType = MimeMapping.GetMimeMapping(caminhoArquivo);
                byte[] bytes = this.LerBytesArquivo(caminhoArquivo);
                byte[] relatorio = bytes;
                var result = (FileContentResult)File(relatorio, mimeType);
                string nomeRelatorio = string.Empty;
                String extensao = this.GetExtensionByMimeType(mimeType);
                if (extensao.Length == 0)
                {
                    throw new Exception("Extensão não encontrada");
                }
                nomeRelatorio = String.Format("arquivo_teste.{0}", extensao);
                result.FileDownloadName = nomeRelatorio;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private String GerarNomeArquivo(String nome)
        {
            return nome;
        }
        private String GetExtensionByMimeType(String mimeType)
        {
            String extensao = String.Empty;
            switch (mimeType)
            {
                case "application/pdf":
                    extensao = "pdf";
                    break;
                case "application/msword":
                    extensao = "doc";
                    break;
                case "image/gif":
                    extensao = "gif";
                    break;
                case "image/png":
                    extensao = "png";
                    break;
                case "image/jpeg":
                    extensao = "jpg";
                    break;
                default:
                    extensao = String.Empty;
                    break;
            }
            return extensao;
        }
        private byte[] LerBytesArquivo(String caminho)
        {
            byte[] bytes = null;
            using (FileStream fsSource = new FileStream(caminho,
                    FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[fsSource.Length];
                int numBytesToRead = (int)fsSource.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                numBytesToRead = bytes.Length;
            }
            byte[] relatorio = bytes;
            return relatorio;
        }
    }
}