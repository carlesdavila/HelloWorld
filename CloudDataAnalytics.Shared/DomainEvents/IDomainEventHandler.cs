namespace CloudDataAnalytics.Shared.DomainEvents
{
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}
