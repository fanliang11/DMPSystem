
using System.Collections.Generic;

namespace DMPSystem.Core.System.Events.EventModel
{
    public class EntityInserted<T>
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
