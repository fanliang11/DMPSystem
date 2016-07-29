using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DMPSystem.Core.EventBus.Configurations;
using DMPSystem.Core.EventBus.DependencyResolution;
using DMPSystem.Core.EventBus.HashAlgorithms;
using DMPSystem.Core.EventBus.Utilities;
using Sys=System;

namespace DMPSystem.Core.EventBus
{
    internal sealed class AppConfig
    {
        #region 字段

        private const string CacheSectionName = "eventProvider";
        private readonly EventWrapperSection _cacheWrapperSetting;
        private static readonly AppConfig _defaultInstance = new AppConfig();

        #endregion

        #region 构造函数

        public AppConfig(Configuration configuration)
            : this(
                configuration.AppSettings.Settings,
                (EventWrapperSection)configuration.GetSection(CacheSectionName))
        {
            DebugCheck.NotNull(configuration);
        }



        private AppConfig()
            : this(
                Convert(ConfigurationManager.AppSettings),
                (EventWrapperSection)ConfigurationManager.GetSection(CacheSectionName))
        {
        }


        private AppConfig(
            KeyValueConfigurationCollection appSettings,
            EventWrapperSection cacheWrapperSetting
            )
        {
            ServiceResolver.Current.Register(null, Activator.CreateInstance(typeof(HashAlgorithm), new object[] { }));
            _cacheWrapperSetting = cacheWrapperSetting ?? new EventWrapperSection();
            RegisterLocalInstance("ISubscriptionService");
            RegisterConfigInstance();

            RegisterConfigInstance(
                this.GetType()
                    .Assembly.GetTypes()
                    .Where(
                        p => p.GetInterface("ISubscriptionAdapt") != null));
       
            InitSettingMethod();
            var bings = _cacheWrapperSetting
                .Queues
                .OfType<QueueCollection>().ToList().Select(p => p.GetTypedPropertyValues()).ToList();
        }

        #endregion

        internal static AppConfig DefaultInstance
        {
            get { return _defaultInstance; }
        }

        public T GetContextInstance<T>() where T : class
        {
            var context = ServiceResolver.Current.GetService<T>(typeof(T));
            return context;
        }

        public T GetContextInstance<T>(string name) where T : class
        {
            DebugCheck.NotEmpty(name);
            var context = ServiceResolver.Current.GetService<T>(name);
            return context;
        }

        #region 私有方法

        private static KeyValueConfigurationCollection Convert(NameValueCollection collection)
        {
            var settings = new KeyValueConfigurationCollection();
            foreach (var key in collection.AllKeys)
            {
                settings.Add(key, ConfigurationManager.AppSettings[key]);
            }
            return settings;
        }

        private void RegisterLocalInstance(string typeName)
        {
            var types = this.GetType().Assembly.GetTypes().Where(p => p.GetInterface(typeName) != null);
            foreach (var t in types)
            {
                var attribute = t.GetCustomAttribute<IdentifyQueueAttribute>();
                ServiceResolver.Current.Register(attribute.Name.ToString(),
                    Activator.CreateInstance(t));
            }

        }

        private void RegisterConfigInstance(IEnumerable<Type>  pTypes=null)
        {
            var bingingSettings = _cacheWrapperSetting.Queues.OfType<QueueCollection>();
            var types = pTypes;
            try
            {
                if (types == null)
                {
                    types =
                        this.GetType()
                            .Assembly.GetTypes()
                            .Where(
                                p => p.GetInterface("IEventPublisher") != null);
                }
                foreach (var t in types)
                {
                    foreach (var setting in bingingSettings)
                    {
                        var properties = setting.GetTypedPropertyValues()
                            .OfType<PropertyElement>();
                        var propertyElements = properties as PropertyElement[] ?? properties.ToArray();
                        var args = propertyElements.Select(p => p.GetTypedPropertyValue())
                            .ToArray();
                        var maps =
                            propertyElements.Select(p => p.GetTypedParameterValues().OfType<MapCollection>())
                                .FirstOrDefault(p => p.Any());
                        var type = setting.GetFactoryType();

                        if (ServiceResolver.Current.GetService(type, setting.IdName) == null)
                            ServiceResolver.Current.Register(setting.IdName, Activator.CreateInstance(type, args));
                        if (maps == null) continue;
                        var mapCollections = maps as MapCollection[] ?? maps.ToArray();
                        if (!mapCollections.Any()) continue;
                        foreach (
                            var mapsetting in
                                mapCollections.Where(mapsetting => t.Name.StartsWith(mapsetting.Name, true,
                                    CultureInfo.InvariantCulture)))
                        {
                            ServiceResolver.Current.Register(string.Format("{0}.{1}", setting.IdName, mapsetting.Name),
                                Activator.CreateInstance(t, new object[] { setting.IdName }));
                        }
                    }
                    var attribute = t.GetCustomAttribute<IdentifyQueueAttribute>();
                    if (attribute != null)
                        ServiceResolver.Current.Register(attribute.Name.ToString(),
                            Activator.CreateInstance(t));
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private void InitSettingMethod()
        {
            var settings =
                _cacheWrapperSetting.Queues.OfType<QueueCollection>()
                    .Where(p => !string.IsNullOrEmpty(p.InitMethodName));
            foreach (var setting in settings)
            {
                var bindingInstance =
                    ServiceResolver.Current.GetService(Type.GetType(setting.ClassName, throwOnError: true),
                        setting.IdName);
                bindingInstance.GetType()
                    .InvokeMember(setting.InitMethodName, BindingFlags.InvokeMethod, null,
                        bindingInstance, new object[] { });
            }
        }

        #endregion
    }
}
