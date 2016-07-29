using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.EventBus.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace DMPSystem.Core.EventBus.Subscription
{
  [IdentifyQueue(name: EventTargetType.PushEvent)]
   public class SubscriptionService:ISubscriptionService
    {
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            var lstService = EnterpriseLibraryContainer.Current.GetAllInstances<IConsumer<T>>();
            if (lstService == null) return null;
            return lstService.ToList();
        }
    }
}
