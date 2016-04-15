

namespace DMPSystem.Core.System.Events.EventModel
{
    public class EntityGeted<T>
    {
        public EntityGeted(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
