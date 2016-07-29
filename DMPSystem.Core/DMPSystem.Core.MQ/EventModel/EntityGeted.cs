

using DMPSystem.Core.EventBus.Rabbit;

namespace DMPSystem.Core.EventBus.EventModel
{
    public class EntityGeted<T>:Event 
    {
        public EntityGeted(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
