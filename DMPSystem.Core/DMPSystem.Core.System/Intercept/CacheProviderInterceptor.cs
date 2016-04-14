/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/

using Castle.DynamicProxy;
using DCLSystem.Core.Caching;
using DCLSystem.Core.Caching.NetCache;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DMPSystem.Core.System.Intercept;

namespace DMPSystem.Core.System.Intercept
{
    /// <summary>
    /// 缓存接口拦截器
    /// </summary>
    public class CacheProviderInterceptor : IInterceptor
    {
        /// <summary>
        /// 缓存KEY
        /// </summary>
        private const string BASEKEY = "{0}_{1}";


        /// <summary>
        /// 拦截执行的方法
        /// </summary>
        /// <param name="invocation">拦截的对象</param>
        public void Intercept(IInvocation invocation)
        {
            object keyIndex = default(object);
            object[] attributes = invocation.Method.GetCustomAttributes(false);
            InterceptMethodAttribute methodAttribute = default(InterceptMethodAttribute);
            LoggerInterceptAttribute loggerAttr = default(LoggerInterceptAttribute);
            foreach (var attribute in attributes)
            {
                if (attribute is InterceptMethodAttribute)
                {
                    methodAttribute = (InterceptMethodAttribute) attribute;
                }
                else if (attribute is LoggerInterceptAttribute)
                {
                    loggerAttr = (LoggerInterceptAttribute) attribute;
                }
            }

            var paramInfos = invocation.Method.GetParameters();
            if (invocation.Arguments.Length > 0)
            {
                keyIndex = invocation.GetArgumentValue(0);
            }
            if (paramInfos.Count() > 0)
            {
                var param = invocation.GetArgumentValue(0);
                if (!(param is IEnumerable))
                {
                    var props = param.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        var propattr = prop.GetCustomAttributes(false);

                        object attr =
                            (from row in propattr where row.GetType() == typeof (CacheKeyAttribute) select row).
                                FirstOrDefault();
                        if (attr == null)
                            continue;

                        keyIndex = prop.GetValue(param, null);
                    }
                }
            }

            #region 缓存处理

            if (methodAttribute != null)
            {
                var keys = new List<string>();

                var key = methodAttribute.Key;
                key = string.IsNullOrEmpty(key)
                          ? string.Format(BASEKEY, invocation.Method.Name, keyIndex)
                          : string.Format(key, keyIndex);
                if (methodAttribute.Method != CachingMethod.Get)
                {
                    var correspondingKeys = methodAttribute.CorrespondingKeys;
                    keys.AddRange(correspondingKeys.Select(correspondingKey => string.Format(correspondingKey, keyIndex)));
                }
                switch (methodAttribute.Mode)
                {
                    case CacheTargetType.WebCache:
                        {
                            CacheIntercept(methodAttribute.Method, invocation, key, methodAttribute.CacheSectionType, methodAttribute.Time);
                            break;
                        }
                    case CacheTargetType.CouchBase:
                        {
                            //CouchBaseIntercept(methodAttribute.Method, invocation, key, methodAttribute.Time,
                            //                   methodAttribute.CacheSectionType, keys);
                            break;
                        }
                }
            }
            else
            {
                invocation.Proceed();
            }

            #endregion

            invocation.ReturnValue = invocation.ReturnValue;
        }

        #region .net 缓存

        private void CacheIntercept(CachingMethod method, IInvocation invocation, string key, SectionType type, int time)
        {
            switch (method)
            {
                case CachingMethod.Get:
                {
                    var cacheObj = CacheContainer.GetInstances<ICacheProvider>(CacheTargetType.WebCache.ToString());
                    var list = cacheObj.Get(key);
                    if (list == null)
                    {
                        invocation.Proceed();
                        list = invocation.ReturnValue;
                        if (list != null)
                        {
                            cacheObj.Add(key, list, time);
                        }
                    }
                    invocation.ReturnValue = list;
                    break;
                }
                case CachingMethod.Put:
                    {

                        break;
                    }
                case CachingMethod.Remove:
                    {

                        break;
                    }
            }
        }

        //#endregion

        //#region CouchBash缓存

        //private void CouchBaseIntercept(CachingMethod method, IInvocation invocation, string key, int time,
        //                               CouchBaseSectionType type, List<string> keys)
        //{
        //    switch (method)
        //    {
        //        case CachingMethod.Get:
        //            {
        //                object list = default(object);
        //                var couchBase = ServiceBase.StaticGetService<ICouchBaseProviderService>();
        //                var json = ServiceBase.StaticGetService<ICouchBaseProviderService>().Get(type, key);
        //                if (string.IsNullOrEmpty(json))
        //                {
        //                    invocation.Proceed();
        //                    list = invocation.ReturnValue;
        //                    if (list != null)
        //                    {
        //                        json = JsonConvert.SerializeObject(list);
        //                        couchBase.Update(json, type, key, time);
        //                    }
        //                }
        //                else
        //                {
        //                    list = JsonConvert.DeserializeObject(json, invocation.Method.ReturnType);
        //                }
        //                invocation.ReturnValue = list;
        //                break;
        //            }
        //        default:
        //            {
        //                var couchBase = ServiceBase.StaticGetService<ICouchBaseProviderService>();
        //                keys.ForEach(p =>
        //                {
        //                    var json = ServiceBase.StaticGetService<ICouchBaseProviderService>().Get(type, p);
        //                    if (!string.IsNullOrEmpty(json))
        //                    {
        //                        couchBase.Update(string.Empty, type, p, time);
        //                    }
        //                });
        //                invocation.Proceed();
        //                break;
        //            }
        //    }
        //}

        #endregion
    }
}
