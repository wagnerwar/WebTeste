using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Batch.Scheduled;
using log4net;
namespace Batch.Servico
{
    public partial class Servico : ServiceBase
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static UtilScheduler prodamScheduller = null;
        //private System.ComponentModel.IContainer components = null;
        public Servico()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                log.Warn("Iniciando o scheduler");
                prodamScheduller = UtilScheduler.Instance;
            }
            catch (Exception e)
            {
                log.Warn("Exceção ao iniciar o Scheduler:" + e.StackTrace);
            }
        }

        protected override void OnStop()
        {
            try
            {
                prodamScheduller.Dispose();
            }
            catch (Exception e)
            {
                log.Warn("Exceção no OnStop do Scheduler:" + e.Message);
            }
        }
    }
}
