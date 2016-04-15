namespace DMPSystem.Core.System.Events.EventPublisher
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage);
    }
}
