using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Utilities
{
      [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IdentifyQueueAttribute : Attribute
    {
        public IdentifyQueueAttribute(EventTargetType name)
        {
            this.Name = name;
        }

        public EventTargetType Name { get; set; }
    }
}
