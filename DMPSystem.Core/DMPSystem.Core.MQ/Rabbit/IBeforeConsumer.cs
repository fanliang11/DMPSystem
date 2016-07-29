using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Rabbit
{
   public interface IBeforeConsumer
    {
        void Excuete<T>(T @event);
    }

}
