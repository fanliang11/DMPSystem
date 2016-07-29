using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Subscription
{
    public interface ISubscriptionService
    {

        IList<IConsumer<T>> GetSubscriptions<T>();
    }
}
