using DMPSystem.Core.EventBus.Rabbit;

namespace DMPSystem.Core.EventBus.Publisher
{
   public interface IEventPublisher
    {
       void Publish<T>(T eventMessage) where T : Event;
    }
}
