using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.EventBus;
using DMPSystem.Core.EventBus.Publisher;

namespace DMPSystem.Service.ConsumerService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public void Debug()
        {
            TaskSet();
        }

        protected override void OnStart(string[] args)
        {
            TaskSet();
        }

        protected override void OnStop()
        {
            
            var publisher = EventContainer.GetInstances<IEventPublisher>("DMPHubEvent.RabbitMq");
            if (publisher != null)
            {
                 
               // publisher.Dispose();
            }
        }

        /// <summary>
        /// 任务启动
        /// </summary>
        public void TaskSet()
        {
             
        }
    }
}
