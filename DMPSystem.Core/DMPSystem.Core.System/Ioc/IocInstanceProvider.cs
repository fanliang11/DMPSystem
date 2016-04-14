/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.System.Ioc
{
    /// <summary>
    /// IOC接口实例服务
    /// </summary>
    public class IocInstanceProvider : IInstanceProvider
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        private readonly Type _serviceType;

        /// <summary>
        /// 接口类型
        /// </summary>
        /// <param name="serviceType"></param>
        public IocInstanceProvider(Type serviceType)
        {
            _serviceType = serviceType;
        }

        #region IInstanceProvider Members

        /// <summary>
        /// 获取接口实例
        /// </summary>
        /// <param name="instanceContext"> 表示服务实例的上下文信息</param>
        /// <param name="message">表示分布式环境中终结点之间的通信单元</param>
        /// <returns>返回接口实例</returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return EnterpriseLibraryContainer.Current.GetInstance(_serviceType);
        }

        /// <summary>
        /// 获取接口实例
        /// </summary>
        /// <param name="instanceContext">表示服务实例的上下文信息</param>
        /// <returns>返回接口实例</returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }
        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="instanceContext">表示服务实例的上下文信息</param>
        /// <param name="instance">需要销毁的对象</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instance is IDisposable)
                ((IDisposable)instance).Dispose();
        }
        #endregion
    }
}
