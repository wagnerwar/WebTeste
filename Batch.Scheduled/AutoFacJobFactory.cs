using Autofac;
using Common.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using Batch.Scheduled.Jobs;
namespace Batch.Scheduled
{
    public class AutoFacJobFactory : IJobFactory
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(AutoFacJobFactory));
        private readonly IContainer _container;
        private Dictionary<string, IContainer> _containers = new Dictionary<string, IContainer>();
        public AutoFacJobFactory(IContainer container = null)
        {
            if (container != null)
                _container = container;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (_container != null)
                return (IJob)_container.Resolve(bundle.JobDetail.JobType);
            else
            {
                var container = CreateContainer(bundle.JobDetail.JobType);
                return (IJob)container.Resolve(bundle.JobDetail.JobType);
            }
        }

        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
            var type = job.GetType();
            if (_containers.ContainsKey(type.FullName))
            {
                var container = _containers[type.FullName];
                _containers.Remove(type.FullName);
                container.Dispose();
            }
        }
        private IContainer CreateContainer(Type type)
        {
            //var builder = Ioc.RegisterModules(false);
            var builder = new ContainerBuilder();
            builder.RegisterType<TarefaService>();
            var container = builder.Build();
            _containers.Add(type.FullName, container);
            return container;
        }
    }
}
