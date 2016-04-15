namespace DMPSystem.Core.System.Events.EventModel
{
    public class EntityDeleted<T>
    {
        public EntityDeleted(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
