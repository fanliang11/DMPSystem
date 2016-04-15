namespace DMPSystem.Core.System.Events.EventModel
{
   public class EntityReported<T>
    {
       public EntityReported(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
