using DMPSystem.Core.EventBus.Rabbit;

namespace DMPSystem.Core.EventBus.EventModel
{
    public class EntityReported<T> : Event
    {
       public EntityReported(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
