using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.EventBus.DependencyResolution;
using DMPSystem.Core.EventBus.HashAlgorithms;
using DMPSystem.Core.EventBus.Publisher;
using DMPSystem.Core.EventBus.Utilities;
using MassTransit;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace DMPSystem.Core.EventBus.Rabbit
{
    [IdentifyQueue(name: EventTargetType.RabbitMq)]
    internal class RabbitMqPublisher : IEventPublisher
    {
        private readonly Lazy<QueueContext> _context;
        private readonly string _appName;

        public RabbitMqPublisher(string appName)
        {
            _context = new Lazy<QueueContext>(() => EventContainer.GetInstance<QueueContext>(appName));
            _appName = appName;
        }

        public RabbitMqPublisher()
        {
            
        }

        public void Publish<T>(T eventMessage) where T : Event
        {
            var node= GetRedisNode(eventMessage.Id.ToString());
             var bus = EventContainer.GetInstance<IBusControl>(string.Format("{0}.{1}",_appName,node.Host));
             bus.Publish(eventMessage);
        }

        private ConsistentHashNode GetRedisNode(string item)
        {
            ConsistentHash<ConsistentHashNode> hash;
            _context.Value.dicHash.TryGetValue(EventTargetType.RabbitMq.ToString(), out hash);
            return hash != null ? hash.GetItemNode(item) : default(ConsistentHashNode);
        }  
    }
}
