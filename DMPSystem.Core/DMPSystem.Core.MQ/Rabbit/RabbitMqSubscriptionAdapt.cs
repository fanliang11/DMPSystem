using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DMPSystem.Core.EventBus.DependencyResolution;
using DMPSystem.Core.EventBus.HashAlgorithms;
using DMPSystem.Core.EventBus.Utilities;
using MassTransit;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ServiceLocation;

namespace DMPSystem.Core.EventBus.Rabbit
{
    [IdentifyQueue(name:EventTargetType.RabbitMq)]
    public class RabbitMqSubscriptionAdapt : ISubscriptionAdapt
    {
         private readonly ContainerBuilder _reg;
         private readonly Lazy<QueueContext> _context;
        private readonly string _appName;

        public RabbitMqSubscriptionAdapt(string appName)
        {
            _context = new Lazy<QueueContext>(() => EventContainer.GetInstance<QueueContext>(appName));
            _appName = appName;
        }

        public RabbitMqSubscriptionAdapt()
        {
            
        }

        /// <summary>
        /// 需要发送Event时
        /// </summary>
        /// <param name="host"></param>
         public void PublishAt()
        {
            var nodes = _context.Value.dicHash[EventTargetType.RabbitMq.ToString()].GetServerNodes();
            foreach (var node in nodes)
            {
                //var endpoint = string.Format("rabbitmq://{0}/{1}", node.Host, node.QueueName);
                var busControl = ConfigureBus(node);
                //busControl.GetSendEndpoint(new Uri(endpoint));
                busControl.Start();
                ServiceResolver.Current.Register(string.Format("{0}.{1}",_appName, node.Host),
                     busControl);
            }
            //_reg.RegisterInstance(busControl).As<IBusControl>().SingleInstance();
            //var publisher = new RabbitMqPublisher(busControl);
            //_reg.RegisterInstance(publisher).Named<IEventPublisher>(EventTargetType.rabbitmq.ToString()).SingleInstance();
        }



        /// <summary>
        /// 需要消费Event
        /// </summary>
        /// <param name="host">队列配置对象</param>
        /// <param name="configurator"></param>
        public void SubscribeAt()
        {
            var nodes = _context.Value.dicHash[EventTargetType.RabbitMq.ToString()].GetServerNodes();
            foreach (var node in nodes)
            {
                var busControl = ConfigureBus(node, new DefaultConsumeConfigurator(ServiceLocator.Current));
                
                busControl.Start();
            }
            //_reg.RegisterInstance(busControl).As<IBusControl>().SingleInstance();
            //var publisher = new RabbitMqPublisher(busControl);

            //_reg.RegisterInstance(publisher).Named<IEventPublisher>(EventTargetType.rabbitmq.ToString()).SingleInstance();
        }

        /// <summary>
        /// 根据Queue的名字查找相应的消费Class
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        private List<Type> GetQueueConsumers(string queueName)
        {
            var result = new List<Type>();
            var consumers = EventContainer.GetInstances(typeof(Subscription.IConsumer));
            foreach (var consumer in consumers)
            {
                var type = consumer.GetType();
                var attributions = type.GetCustomAttributes(typeof(QueueConsumerAttribute), false);
                if (attributions.Length <= 0)
                {
                    result.Add(type);
                    continue;
                }
                var attribution = attributions[0] as QueueConsumerAttribute;
                if (attribution == null || attribution.QueueName.Equals(queueName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(type);
                }
            }
            return result;
        }

        private IBusControl ConfigureBus(ConsistentHashNode phost, IConsumeConfigurator configurator = null)
        {
            var endpoint = string.Format("rabbitmq://{0}/", phost.Host); 
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(endpoint), h =>
                {
                    h.Username(phost.UserName);
                    h.Password(phost.Password);
                });
                cfg.Durable = true;
                cfg.UseRateLimit(int.Parse(phost.UseRateLimit), TimeSpan.FromSeconds(1));//每分钟消息消费数限定在1000之内
                cfg.UseConcurrencyLimit(int.Parse(phost.CunsumerNum));
                cfg.UseRetry(Retry.Interval(int.Parse(phost.UseRetryNum), TimeSpan.FromMinutes(1)));//消息消费失败后重试3次，每次间隔1分钟
                if (configurator != null)
                {
                    var consumers = GetQueueConsumers(phost.QueueName);
                    cfg.ReceiveEndpoint(host, phost.QueueName, eq => configurator.Configure(eq, consumers));
                }

            });
        }

    }
}
