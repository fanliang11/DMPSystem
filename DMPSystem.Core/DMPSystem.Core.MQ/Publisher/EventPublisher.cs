using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.EventBus.Rabbit;
using DMPSystem.Core.EventBus.Subscription;
using DMPSystem.Core.EventBus.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace DMPSystem.Core.EventBus.Publisher
{
    [IdentifyQueue(name: EventTargetType.PushEvent)]
    internal class EventPublisher : IEventPublisher
    {

        private readonly Lazy<ISubscriptionService> _subscriptionService;

        public EventPublisher()
        {
            _subscriptionService = new Lazy<ISubscriptionService>(()=>EventContainer.GetInstances<ISubscriptionService>(EventTargetType.PushEvent.ToString()));
        }

        public void Publish<T>(T eventMessage) where T : Event
        {
            var subscriptions = _subscriptionService.Value.GetSubscriptions<T>();
            if (subscriptions == null) return;
            subscriptions.ToList().ForEach(x => PublishToConsumer(x, eventMessage));
        }

        private static void PublishToConsumer<T>(IConsumer<T> x, T eventMessage)
        {
            try
            {
                x.HandleEvent(eventMessage);
            }
            catch (Exception ex)
            {
                throw ex;
               // ExceptionHelper.LogException(ex, HttpContext.Current, null, "EventPublisher");
            }
            finally
            {
                //TODO actually we should not dispose it
                var instance = x as IDisposable;
                if (instance != null)
                {
                    instance.Dispose();
                }
            }
        }
    }
}
