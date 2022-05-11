using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Batch.Console
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Batch.Console.Program));
        static Batch.Scheduled.UtilScheduler prodamScheduller = null;
        static void Main(string[] args)
        {
            try
            {
                log.Warn(".");
                log.Warn(".");
                log.Warn(".");
                log.Warn("--- Instanciando o Scheduler ------");
                prodamScheduller = Batch.Scheduled.UtilScheduler.Instance;
                System.Console.ReadLine();                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        static public void Finalizar()
        {
            try
            {
                log.Warn("--- Finalizando o Scheduler ------");
                prodamScheduller.Dispose();

            }
            catch
            {
                log.Warn("--- Erro na finalização do Scheduler ------");
            }
        }
    }
}
