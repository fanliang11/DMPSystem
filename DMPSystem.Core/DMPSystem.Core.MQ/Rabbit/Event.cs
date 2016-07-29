using System;
using config = System.Configuration;

namespace DMPSystem.Core.EventBus.Rabbit
{
    public interface IEvent
    {
        /// <summary>
        /// 事件唯一码
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; }

        /// <summary>
        /// 发送事件的服务器名
        /// </summary>
        string Machine { get; }

        /// <summary>
        /// 通过那台
        /// </summary>
        string FromApplication { get; }
    }

    public abstract class Event : IEvent
    {
        public static readonly string DefaultApplicationName = config.ConfigurationManager.AppSettings["AppName"];

        public static readonly string DefaultMachineName = Environment.MachineName;

        protected Event()
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            Machine = DefaultMachineName;
            FromApplication = DefaultApplicationName;
        }

        public Guid Id { get; private set; }


        public DateTime CreateTime { get; private set; }


        public string Machine { get; private set; }


        public string FromApplication { get; private set; }
    }
}

