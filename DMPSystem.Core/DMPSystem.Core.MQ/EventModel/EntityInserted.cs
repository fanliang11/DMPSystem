
using System.Collections.Generic;
using DMPSystem.Core.EventBus.Rabbit;

namespace DMPSystem.Core.EventBus.EventModel
{
    public class EntityInserted<T> : Event
    {
        public EntityInserted(T entity)
        {
            this.Entity = entity;
        }

        public EntityInserted(T entity,Dictionary<string,string> fieldValues)
        {
            this.Entity = entity;
            this.FieldValues = fieldValues;
        }
        public Dictionary<string, string> FieldValues { get; set; }
        public T Entity { get; private set; }
    }
}
