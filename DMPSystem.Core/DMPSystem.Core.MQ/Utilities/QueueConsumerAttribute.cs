using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Utilities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class QueueConsumerAttribute : Attribute
    {
        public string QueueName
        {
            get { return _queueName; }
        }

        private string _queueName { get; set; }

        public QueueConsumerAttribute(string queueName)
        {
            _queueName = queueName;
        }

    }
}
