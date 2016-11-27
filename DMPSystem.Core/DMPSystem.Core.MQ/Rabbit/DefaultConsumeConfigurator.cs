using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DMPSystem.Core.EventBus.Subscription;
using Magnum.Reflection;
using MassTransit;
using MassTransit.Configurators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ServiceLocation;

namespace DMPSystem.Core.EventBus.Rabbit
{
    public class DefaultConsumeConfigurator : IConsumeConfigurator
    {
        private readonly IServiceLocator _locator;
        private IBeforeConsumer _beforeConsumer;
        private IAfterConsumer _afterConsumer;

        public DefaultConsumeConfigurator(IServiceLocator locator)
        {
            _locator = locator;
        }

        private void InitConsumer()
        {
            try
            {
                _beforeConsumer = _locator.GetInstance<IBeforeConsumer>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                _afterConsumer = _locator.GetInstance<IAfterConsumer>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Configure(IRabbitMqReceiveEndpointConfigurator cfg, List<Type> consumers)
        {
            //InitConsumer();
            foreach (var consumer in consumers)
            {
                if (consumer.IsGenericType)
                {
                    continue;
                }
                var consumerTypes = consumer.GetInterfaces()
                    .Where(
                        d =>
                            d.IsGenericType &&
                            d.GetGenericTypeDefinition() == typeof (Subscription.IConsumer<>))
                    .Select(d => d.GetGenericArguments().Single())
                    .Distinct();

                foreach (var eventconsumerType in consumerTypes)
                {
                    try
                    {
                        var type = consumer;
                        this.FastInvoke(new[] {eventconsumerType, consumer},
                            x => x.ConsumerTo<object, Subscription.IConsumer<object>>(cfg, type),
                            cfg,
                            consumer);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        protected void ConsumerTo<TEvent, TConsumer>(IRabbitMqReceiveEndpointConfigurator cfg, Type handlerType)
            where TConsumer : Subscription.IConsumer<TEvent>
            where TEvent : class 
        {
            cfg.Handler<TEvent>(async evnt =>
            {
                try
                {
                    await Task.Run(() =>
                    {
                        var source = new TaskCompletionSource<TEvent>();
                        if (_beforeConsumer != null)
                            _beforeConsumer.Excuete(evnt.Message);
                        MethodInfo method = handlerType.GetMethod("HandleEvent", new Type[] { evnt.Message.GetType()});
                        method.Invoke(Activator.CreateInstance(handlerType, new object[] { }), new object[] { evnt.Message });
                        if (_afterConsumer != null)
                            _afterConsumer.Excuete(evnt.Message);
                              return Task.FromResult(true);
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
    }
}
