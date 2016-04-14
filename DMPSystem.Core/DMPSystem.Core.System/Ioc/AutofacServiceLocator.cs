/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/
using Autofac;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.System.Ioc
{
    /// <summary>
    ///  依赖Service Locator模式进行Autofac
    /// </summary>
    public sealed class AutofacServiceLocator : ServiceLocatorImplBase
    {
        private readonly IComponentContext _container;
        /// <summary>
        /// 构造autofac容器
        /// </summary>
        /// <param name="container">ioc容器</param>
        public AutofacServiceLocator(IComponentContext container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            ServiceLocator.SetLocatorProvider(() => this);
            _container = container;
        }

        /// <summary>
        /// 返回实例化的对象
        /// </summary>
        /// <param name="serviceType">需要反转的类型</param>
        /// <param name="key">获取元素的建</param>
        /// <returns>返回实例化对象</returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {

            return key != null ? _container.ResolveNamed(key, serviceType) : _container.Resolve(serviceType);
        }

        /// <summary>
        /// 返回实例化对象列表
        /// </summary>
        /// <param name="serviceType">需要反转的类型</param>
        /// <returns>返回实例化对象列表</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);

            object instance = _container.Resolve(enumerableType);
            return ((IEnumerable)instance).Cast<object>();
        }
    }
}
