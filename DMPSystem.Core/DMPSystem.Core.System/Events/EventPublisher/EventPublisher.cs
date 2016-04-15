#region

using System;
using System.Linq;
using System.Web;
using DMPSystem.Core.Common.ServicesException;
using DMPSystem.Core.System.Events.EventPublisher;
using DMPSystem.Core.System.Events.Subscription;


#endregion

namespace DMPSystem.Core.System.Events.EventPublisher
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ISubscriptionService _subscriptionService;

        public EventPublisher(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public void Publish<T>(T eventMessage)
        {
            var subscriptions = _subscriptionService.GetSubscriptions<T>();
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
               throw new  ServiceException("",ex);
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
