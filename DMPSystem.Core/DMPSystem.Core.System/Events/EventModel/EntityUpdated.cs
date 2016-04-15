
using System.Collections.Generic;


namespace DMPSystem.Core.System.Events.EventModel
{
    public class EntityUpdated<T>
    {
        public EntityUpdated(T entity)
        {
            this.Entity = entity;
        }

        public EntityUpdated(T entity, T oldEntity)
        {
            this.Entity = entity;
            this.OldEntity = oldEntity;
        }

        public EntityUpdated(T entity, T oldEntity, Dictionary<string, string> fieldValues)
        {
            this.Entity = entity;
            this.OldEntity = oldEntity;
            this.FieldValues = fieldValues;
        }

        public T Entity { get; private set; }

        public T OldEntity { get; private set; }

        public Dictionary<string, string> FieldValues { get; set; }
    }
}
