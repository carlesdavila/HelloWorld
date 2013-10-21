using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace CloudDataAnalytics.Shared.DomainEvents
{
    public class ScheduledDomainEventBase :  IScheduledDomainEvent
    {
        public IJobExecutionContext JobExecutionContext { get; set; }
    }
}
