using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Data;
using Microsoft.Reporting.WebForms;
namespace Reports
{
    public class Report
    {
        public  const string REP_TESTE = "Reports.Rdlc.TesteReport.rdlc";
        private string _reportName;
        private ReportType _reportType;
        private Dictionary<string, object> _reportParameters;
        private List<DataSetRelatorio> _listaDataSetRelatorio;
        public Report(List<DataSetRelatorio> listaDataSetRelatorio, Dictionary<string, object> reportParameters, string reportName, ReportType reportType)
        {
            _listaDataSetRelatorio = listaDataSetRelatorio;
            _reportParameters = reportParameters;
            _reportName = reportName;
            _reportType = reportType;
        }
        public byte[] Generate(out string mimeType)
        {

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("pt-BR");
                ReportViewer rv = new ReportViewer();
                rv.LocalReport.ReportEmbeddedResource = _reportName;
                rv.LocalReport.DataSources.Clear();

                var reportDataSources = new List<ReportDataSource>();

                foreach (DataSetRelatorio dataSetRelatorio in _listaDataSetRelatorio)
                {
                    reportDataSources.Add(new ReportDataSource(dataSetRelatorio.dataSetName, dataSetRelatorio.dataTable));
                }

                foreach (ReportDataSource ds in reportDataSources)
                    rv.LocalReport.DataSources.Add(ds);

                if (_reportParameters != null)
                {
                    var parameters = new List<ReportParameter>();
                    foreach (var item in _reportParameters)
                    {
                        ReportParameter p = new ReportParameter { Name = item.Key };
                        p.Values.Add(item.Value.ToString());
                        parameters.Add(p);
                    }
                    if (parameters != null && parameters.Count > 0)
                    {
                        rv.LocalReport.SetParameters(parameters.ToArray());
                    }
                }

                //Para não dar erro de permissão teria que alterar o web.config de C:\Windows\Microsoft.NET\Framework\v4.0.30319\Config, mas a arquitetura optou por colocar essa linha, pois é uma permissão do projeto
                rv.LocalReport.SetBasePermissionsForSandboxAppDomain(new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted));

                rv.LocalReport.Refresh();

                string encoding = "";
                string fileNameExtension = "";
                Warning[] warnings = null;
                string[] streamids = null;

                byte[] exportBytes = rv.LocalReport.Render(_reportType.ToString(), null, out mimeType, out encoding, out fileNameExtension, out streamids, out warnings);

                return exportBytes;
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message, erro.InnerException);
            }
        }
    }
    public enum ReportType
    {
        PDF = 1,
        Excel = 2
    }
    public class DataSetRelatorio
    {
        public DataTable dataTable;

        public string dataSetName;
    }
}
