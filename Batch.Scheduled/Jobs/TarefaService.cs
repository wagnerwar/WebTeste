using Autofac;
using Common.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using Batch.Dominio;
namespace Batch.Scheduled.Jobs
{
    public class TarefaService : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TarefaService));
        public void Execute(IJobExecutionContext context)
        {
            logger.Info("Iniciando tarefa");
            Teste dado = new Teste();
            dado.Nome = "Teste";
            logger.Info("Finalizando tarefa");
        }
        private void NotificacaoProcessamento(string acao, bool final = false)
        {
            logger.Warn("EFETUAR_BAIXA_GUIAS_JOB: - " + acao);
        }
    }
}
