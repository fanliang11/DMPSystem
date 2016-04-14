/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.System.Ioc
{
    /// <summary>
    /// 实现自定义扩展
    /// </summary>
    public class IocServiceBehavior : Attribute, IServiceBehavior
    {
        #region IServiceBehavior Members
        /// <summary>
        /// 用于向绑定元素传递自定义数据，以支持协定实现。
        /// </summary>
        /// <param name="serviceDescription">   服务的服务说明。</param>
        /// <param name="serviceHostBase">当前正在生成的宿主。</param>
        /// <param name="endpoints"> 服务终结点。</param>
        /// <param name="bindingParameters">绑定元素可访问的自定义对象。</param>
        public void AddBindingParameters(ServiceDescription serviceDescription,
                                         ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// 用于更改运行时属性值或插入自定义扩展对象（例如错误处理程序、消息或参数拦截器、安全扩展以及其他自定义扩展对象）。
        /// </summary>
        /// <param name="serviceDescription"> 服务说明。</param>
        /// <param name="serviceHostBase"> 当前正在生成的宿主。</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription,
                                          ServiceHostBase serviceHostBase)
        {
        
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher ed in cd.Endpoints)
                {
                    if (!ed.IsSystemEndpoint)
                    {
                       cd.ErrorHandlers.Add(new WcfErrorHandler());
                        ed.DispatchRuntime.InstanceProvider =
                            new IocInstanceProvider(serviceDescription.ServiceType);
                    }
                }
            }
        }

        /// <summary>
        ///   用于检查服务宿主和服务说明，从而确定服务是否可成功运行。
        /// </summary>
        /// <param name="serviceDescription">服务说明。</param>
        /// <param name="serviceHostBase"> 当前正在构建的服务宿主。</param>
        public void Validate(ServiceDescription serviceDescription,
                             ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}
