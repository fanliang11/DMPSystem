using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Configurations
{
    public sealed class EventWrapperSection : ConfigurationSection
    {
        #region 字段
        private const string ProviderKey = "providers";
        private const string QueueKey = "queues";
        #endregion

        #region 属性
        [ConfigurationProperty(ProviderKey,IsRequired = false)]
        public ProviderCollection Providers
        {
            get { return (ProviderCollection)base[ProviderKey]; }
        }

        [ConfigurationProperty(QueueKey, IsRequired = false)]
        public QueuesCollection Queues
        {
            get { return (QueuesCollection)base[QueueKey]; }
        }
        #endregion
    }
}
