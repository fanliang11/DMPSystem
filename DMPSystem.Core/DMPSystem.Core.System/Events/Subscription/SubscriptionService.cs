using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.System.Events.Subscription
{
    public class SubscriptionService : ServiceBase, ISubscriptionService
    {
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            var lstService = GetServices<IConsumer<T>>();
            return lstService == null ? null : lstService.ToList();
        }
    }
}
