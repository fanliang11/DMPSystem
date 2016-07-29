using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DMPSystem.Core.EventBus;
using DMPSystem.Core.System.Ioc;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace DMPSystem.Service.ConsumerService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new Service1() 
            //};
            //ServiceBase.Run(ServicesToRun);
            var builder = new ContainerBuilder();
            builder.Initialize();
            builder.RegisterServices();
            builder.RegisterRepositories();
            builder.RegisterBusinessModules();
            builder.RegisterModules();
            var container = builder.Build();

            EnterpriseLibraryContainer.Current = new AutofacServiceLocator(container);
            builder.InitializeModule();
            EventContainer.GetInstances<ISubscriptionAdapt>("DMPHubEvent.RabbitMq").SubscribeAt();
            new Service1().Debug();
        }
    }
}
