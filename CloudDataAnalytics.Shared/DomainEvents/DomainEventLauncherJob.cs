using System;
using Quartz;

namespace CloudDataAnalytics.Shared.DomainEvents
{
    public class DomainEventLauncherJob<T> : IJob 
        where T : IScheduledDomainEvent, new()
    {
        public void Execute(IJobExecutionContext context)
        {
            var evt = new T {JobExecutionContext = context };
            DomainEventDispatcher.Raise(evt);
        }
    }
}
