using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Rabbit
{
    public class RabbitMQHost
    {
        public string Host { get; set; }
        public string QueueName { get; set; }
        public int CunsumerNum { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IConsumeConfigurator ConsumeConfigurator { get; set; }
    }
}
