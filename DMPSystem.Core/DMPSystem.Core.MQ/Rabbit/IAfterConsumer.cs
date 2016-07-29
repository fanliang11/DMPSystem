using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Rabbit
{
    public interface IAfterConsumer
    {
        void Excuete<T>(T @event);
    }
}
