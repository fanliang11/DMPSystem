/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/

using System.IO;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;
using Castle.DynamicProxy;
using DMPSystem.Core.System.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Extras.DynamicProxy2;
using DMPSystem.Core.System.Module;
using DMPSystem.Core.System.Module.Attributes;
using DMPSystem.Core.Common.ServicesException;
using Autofac.Core.Registration;

namespace DMPSystem.Core.System.Ioc
{
    public static class RegistrationExtensions
    {
        private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();
        private static readonly IEnumerable<Service> EmptyServices = new Service[0];

        private const string InterceptorsPropertyName =
            "DMPSystem.Core.System.Ioc.RegistrationExtensions.InterceptorsPropertyName";

        private static Dictionary<string, Assembly> _referenceAssembly = new Dictionary<string, Assembly>();

        public static List<Assembly> GetReferenceAssembly(this ContainerBuilder builder)
          {
              return _referenceAssembly.Values.ToList();
          }

        public static void Initialize(this ContainerBuilder builder,
                                      string pattern = "DMPSystem")
        {

            string path = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
            if (_referenceAssembly.Count == 0)
            {
                var assemblyNames = GetAllAssemblyFiles(path, pattern);
                foreach (var referencedAssemblyName in assemblyNames)
                {
                    var referencedAssembly = Assembly.Load(referencedAssemblyName);
                    if (
                        referencedAssembly.CustomAttributes.Any(
                            p => p.AttributeType == typeof (AssemblyModuleTypeAttribute)))
                    {
                        _referenceAssembly.Add(referencedAssemblyName, referencedAssembly);
                    }

                }
            }
        }

        /// <summary>
        /// 依赖注入注册wcf相关的程序集
        /// </summary>
        /// <param name="builder">ioc容器</param>
        /// <returns>返回注册模块信息</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterWcfServices(
            this ContainerBuilder builder)
        {
            var referenceAssemblies= builder.GetReferenceAssembly();
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> result = null;
            if (builder == null) throw new ArgumentNullException("builder");
            var interfaceAssemblies = referenceAssemblies.Where(
                p =>
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.WcfService).ToList();
            foreach (var interfaceAssembly in interfaceAssemblies)
            {
                result = builder.RegisterAssemblyTypes(interfaceAssembly)
                    .Where(t => t.Name.EndsWith("Service")).SingleInstance();

            }
            return result;

        }

        /// <summary>
        /// 依赖注入业务模块程序集
        /// </summary>
        /// <param name="builder">ioc容器</param>
        /// <returns>返回注册模块信息</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterServices(
            this ContainerBuilder builder)
        {
            var referenceAssemblies = builder.GetReferenceAssembly();
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> result = null;
            if (builder == null) throw new ArgumentNullException("builder");

            #region 接口服务注入

            builder.RegisterType<CacheProviderInterceptor>();
            var interfaceAssemblies = referenceAssemblies.Where(
                p =>
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.InterFaceService ||
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.SystemModule).ToList();
            foreach (var interfaceAssembly in interfaceAssemblies)
            {
                result = builder.RegisterAssemblyTypes(interfaceAssembly)
                    .Where(t => t.Name.StartsWith("I")).Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces().EnableInterfaceInterceptors();

            }

            #endregion

            #region 领域服务注入

            var domainAssemblies = referenceAssemblies.Where(
                p =>
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.BusinessModule ||
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.Domain).ToList();
            foreach (var domainAssembly in domainAssemblies)
            {
                result = builder.RegisterAssemblyTypes(domainAssembly)
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces().EnableInterfaceInterceptors();
            }

            #endregion

            return result;
        }

        /// <summary>
        ///依赖注入仓储模块程序集
        /// </summary>
        /// <param name="builder">IOC容器</param>
        /// <returns>返回注册模块信息</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterRepositories
            (
            this ContainerBuilder builder)
        {

            var referenceAssemblies = builder.GetReferenceAssembly();
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> result = null;
            if (builder == null) throw new ArgumentNullException("builder");
            var repositoryAssemblies = referenceAssemblies.Where(
                p =>
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.BusinessModule ||
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.Domain).ToList();
            foreach (var repositoryAssembly in repositoryAssemblies)
            {
                result = builder.RegisterAssemblyTypes(repositoryAssembly)
                    .Where(t => t.Name.EndsWith("Repository"))
                    .EnableClassInterceptors().InterceptedBy(typeof (CacheProviderInterceptor));
            }
            return result;
        }

        public static IModuleRegistrar RegisterModules(
            this ContainerBuilder builder)
        {
            var referenceAssemblies = builder.GetReferenceAssembly();
            IModuleRegistrar result = default(IModuleRegistrar);
            if (builder == null) throw new ArgumentNullException("builder");
            var moduleAssemblies = referenceAssemblies.Where(
                p =>
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.SystemModule ||
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.BusinessModule).ToList();
            foreach (var moduleAssembly in moduleAssemblies)
            {
                result = builder.RegisterModules(moduleAssembly);
            }
            return result;
        }

        private static IModuleRegistrar RegisterModules(
            this ContainerBuilder builder, Assembly assembly)
        {
            IModuleRegistrar result = default(IModuleRegistrar);

            GetAbstractModules(assembly).ForEach(p =>
                                                     {
                                                         result = builder.RegisterModule(p);
                                                     });

            return result;
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterBusinessModules(this ContainerBuilder builder)
        {
            var referenceAssemblies = builder.GetReferenceAssembly();
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> result = null;
            if (builder == null) throw new ArgumentNullException("builder");
            var repositoryAssemblies = referenceAssemblies.Where(
                p =>
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.BusinessModule ||
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.Domain).ToList();
            foreach (var repositoryAssembly in repositoryAssemblies)
            {
                result = builder.RegisterAssemblyTypes(repositoryAssembly).AsImplementedInterfaces().SingleInstance();
            }
            return result;
        }

   
        public static void InitializeModule(this ContainerBuilder builder)
        {
            var referenceAssemblies = builder.GetReferenceAssembly();
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> result = null;
            if (builder == null) throw new ArgumentNullException("builder");
            var moduleAssemblies = referenceAssemblies.Where(
                p =>
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.SystemModule ||
                p.GetCustomAttribute<AssemblyModuleTypeAttribute>().Type == ModuleType.BusinessModule).ToList();
            foreach (var moduleAssembly in moduleAssemblies)
            {
                GetAbstractModules(moduleAssembly).ForEach(p => p.Initialize());
            }
        }

        private static List<AbstractModule> GetAbstractModules(Assembly assembly)
        {
            var abstractModules = new List<AbstractModule>();
            Type[] arrayModule =
                assembly.GetTypes().Where(
                    t => t.IsSubclassOf(typeof (AbstractModule)) && t.Name.EndsWith("Module")).ToArray();
            foreach (var moduleType in arrayModule)
            {
                #region 模块描述 Attribute

                var moduleDescriptionAttribute =
                    GetTypeCustomAttribute<ModuleDescriptionAttribute>(moduleType);
                if (moduleDescriptionAttribute == null)
                {
                    throw new ServiceException(string.Format("{0} 模块没有定义 ModuleDescriptor 特性",
                                                             moduleType.AssemblyQualifiedName));
                }

                #endregion

                var abstractModule = (AbstractModule) Activator.CreateInstance(moduleType);
                abstractModules.Add(abstractModule);
            }
            return abstractModules;
        }

        /// <summary>
        /// 获取指定类型的自定义特性对象。
        /// </summary>
        /// <typeparam name="TAttributeType">类型参数：自定义特性的类型。</typeparam>
        /// <param name="type">类型。</param>
        /// <returns>返回指定类型的自定义特性对象。</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2015/12/5</para>
        /// </remarks>
        public static TAttributeType GetTypeCustomAttribute<TAttributeType>(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof (TAttributeType), true);

            if (attributes.Length > 0)
            {
                return (TAttributeType) attributes[0];
            }

            return default(TAttributeType);
        }

        private static List<string> GetAllAssemblyFiles(string parentDir, string pattern)
        {
            return
                Directory.GetFiles(parentDir, "*.dll").Select(Path.GetFileNameWithoutExtension).Where(
                    a => Regex.IsMatch(a, pattern)).ToList();
        }
    }
}
