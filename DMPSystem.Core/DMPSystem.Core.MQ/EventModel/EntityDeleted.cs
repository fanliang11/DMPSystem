using DMPSystem.Core.EventBus.Rabbit;

namespace DMPSystem.Core.EventBus.EventModel
{
    public class EntityDeleted<T> : Event
    {
        public EntityDeleted(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
