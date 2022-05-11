using System;
using Common.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Autofac;
using Quartz.Spi;

namespace Batch.Scheduled
{
    public class UtilScheduler : IDisposable
    {
        ILog log = LogManager.GetLogger(typeof(UtilScheduler));
        public IScheduler _scheduler = null;
        private static UtilScheduler instance;
        IContainer _container;
        public static UtilScheduler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UtilScheduler();
                }
                return instance;
            }
        }
        private UtilScheduler()
        {
            //Criação do container aqui, criou problema de concorrência entre os jobs e o uow
            //var builder = IoC.RegisterModules(false);
            //builder.RegisterType<ProcessarPermissoesJob>();
            //builder.RegisterType<EfetuarBaixaGuiasJob>();
            //_container = builder.Build();
            // IJobFactory jobFactory = new AutoFacJobFactory(_container);

            IJobFactory jobFactory = new AutoFacJobFactory();
            ISchedulerFactory factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler();
            _scheduler.JobFactory = jobFactory;

            log.Warn("--- Iniciando o Quartz ---");
            _scheduler.Start();

            if ((_scheduler.IsStarted))
            {
                log.Warn("*************************--- Verificando jobs do Quartz ---");
                foreach (string jobGroup in _scheduler.GetJobGroupNames())
                {
                    var groupMatcher = GroupMatcher<JobKey>.GroupContains(jobGroup);
                    var jobKeys = _scheduler.GetJobKeys(groupMatcher);
                    foreach (var jobKey in jobKeys)
                    {
                        var detail = _scheduler.GetJobDetail(jobKey);
                        var triggers = _scheduler.GetTriggersOfJob(jobKey);
                        foreach (ITrigger trigger in triggers)
                        {
                            log.Warn("*" + jobKey.Name);
                            //log.Warn("*Job: " + jobGroup);
                            //log.Warn("*" + detail.Description);
                            //log.Warn("*" + trigger.Key.Name);
                            //log.Warn("*" + trigger.Key.Group);
                            //log.Warn("*" + trigger.GetType().Name);
                            //log.Warn("*" + _scheduler.GetTriggerState(trigger.Key));
                            DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();

                            if (nextFireTime.HasValue)
                            {
                                log.Warn("* Próxima Execução ---" + nextFireTime.Value.LocalDateTime.ToString());
                            }

                            DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
                            if (previousFireTime.HasValue)
                            {
                                log.Warn("* Execução Anterior ---" + previousFireTime.Value.LocalDateTime.ToString());
                            }
                        }
                    }
                }
                log.Warn("*************************--- Finalizando  listagem de job do Quartz ---");
            }
        }
        public void Dispose()
        {
            if (_container != null)
                _container.Dispose();

            if (_scheduler != null)
            {
                log.Warn("--- Iniciando o desligamento do quartz-------");
                _scheduler.Shutdown(true);
                _scheduler = null;
                log.Warn("--- Concluiu o desligamento do quartz-------");
            }
        }
    }
}
