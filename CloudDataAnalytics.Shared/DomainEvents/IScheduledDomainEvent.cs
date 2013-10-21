using Quartz;

namespace CloudDataAnalytics.Shared.DomainEvents
{
    public interface IScheduledDomainEvent : IDomainEvent
    {
        IJobExecutionContext JobExecutionContext { get; set; }
    }
}
