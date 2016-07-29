using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace DMPSystem.Core.EventBus.Rabbit
{
    public interface IConsumeConfigurator
    {
        void Configure(IRabbitMqReceiveEndpointConfigurator cfg, List<Type> consumers);
    }
}
