/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-04-12      创建
 ----------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.Common.ServicesException;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace DMPSystem.Core.System
{
    /// <summary>
    /// 领域服务基类
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>
        /// 根据类型获取实例
        /// </summary>
        /// <typeparam name="TReturn">实例</typeparam>
        /// <returns>返回实例</returns>
        public TReturn GetService<TReturn>()
        {
            return EnterpriseLibraryContainer.Current.GetInstance<TReturn>();
        }

        /// <summary>
        /// 根据实例名称获取相关实例
        /// </summary>
        /// <typeparam name="TReturn">返回的类型</typeparam>
        /// <param name="name">实例名</param>
        /// <returns>返回实例</returns>
        public TReturn GetServices<TReturn>(string name)
        {
            return
                EnterpriseLibraryContainer.Current.GetAllInstances<TReturn>()
                    .FirstOrDefault(t => t.GetType().Name == name);
        }

        /// <summary>
        /// 根据实例名称获取相关实例
        /// </summary>
        /// <typeparam name="TReturn">返回的类型</typeparam>
        /// <param name="key">实例名</param>
        /// <returns>返回实例</returns>
        public TReturn GetServiceByKey<TReturn>(string key)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<TReturn>(key);
        }

        /// <summary>
        ///  获取全部实例
        /// </summary>
        /// <typeparam name="TReturn">实例列表</typeparam>
        /// <returns>返回实例</returns>
        public IEnumerable<TReturn> GetServices<TReturn>()
        {
            return EnterpriseLibraryContainer.Current.GetAllInstances<TReturn>();
        }

        /// <summary>
        /// 根据类型获取实例
        /// </summary>
        /// <typeparam name="TReturn">实例</typeparam>
        /// <returns>返回实例</returns>
        public static TReturn StaticGetService<TReturn>()
        {
            return EnterpriseLibraryContainer.Current.GetInstance<TReturn>();
        }

        /// <summary>
        /// 根据实例名称获取相关实例
        /// </summary>
        /// <typeparam name="TReturn">返回的类型</typeparam>
        /// <param name="key">实例名</param>
        /// <returns>返回实例</returns>
        public static TReturn StaticGetServiceByKey<TReturn>(string key)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<TReturn>(key);
        }

        /// <summary>
        ///  获取全部实例
        /// </summary>
        /// <typeparam name="TReturn">实例列表</typeparam>
        /// <returns>返回实例</returns>
        public static IEnumerable<TReturn> StaticGetServices<TReturn>()
        {
            return EnterpriseLibraryContainer.Current.GetAllInstances<TReturn>();
        }

        /// <summary>
        ///定义一个帮助器类，在与对象、属性和方法关联的 System.ComponentModel.DataAnnotations.ValidationAttribute
        /// 特性中包含此类时，可使用此类来验证这些项。
        /// </summary>
        /// <param name="instance"> 要验证的对象。</param>
        /// <returns>返回验证请求结果的容器列表</returns>
        public static List<ValidationResult> Validation(object instance)
        {
            var context = new ValidationContext(instance);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(instance, context, results, true);
            return results;
        }

        /// <summary>
        ///定义一个帮助器类，在与对象、属性和方法关联的 System.ComponentModel.DataAnnotations.ValidationAttribute
        /// 特性中包含此类时，可使用此类来验证这些项。
        /// </summary>
        /// <param name="instance">要验证的对象。</param>
        /// <param name="serviceProvider">指定类型的服务对象。</param>
        /// <param name="items"> 要提供给服务使用方的键/值对的字典。 此参数是可选的。</param>
        /// <returns>返回验证请求结果的容器列表</returns>
        public static List<ValidationResult> Validation(object instance, IServiceProvider serviceProvider,
            IDictionary<object, object> items)
        {
            var context = new ValidationContext(instance, serviceProvider, items);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(instance, context, results, true);
            return results;
        }

        /// <summary>
        /// 捕获验证的自定义错误
        /// </summary>
        /// <param name="o">需要验证的实体</param>
        protected void ThrowValidationError<T>(object o)
        {
            var result = Validation(o);
            if (result.Count > 0)
            {
                throw new ServiceException(result[0].ErrorMessage).GetServiceException<T>();
            }
        }

    }
}
