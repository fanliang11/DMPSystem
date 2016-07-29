using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Configurations
{
    /// <summary>
    /// PropertyElement节点集合的配置元素。
    /// </summary>
    /// <remarks>
    /// 	<para>创建：范亮</para>
    /// 	<para>日期：2016/4/2</para>
    /// </remarks>
    public sealed class PropertyElement : ConfigurationElementCollection
    {
        #region 字段
        private static readonly ConfigurationPropertyCollection PropertyCollection = new ConfigurationPropertyCollection();
        private const string NameKey = "name";
        private const string ValueKey = "value";
        private const string MapKey = "map";
        private const string RefKey = "ref";
        private const string TypeKey = "type";
        private const string ProviderRefKey = "provider-ref";
        #endregion

        #region 友元方法
        /// <summary>
        ///    创建<see cref="MapCollection"/> 元素
        /// </summary>
        /// <returns>返回一个新的 System.Configuration.ConfigurationElement。</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        protected override ConfigurationElement CreateNewElement()
        {
            var element = new MapCollection();
            return element;
        }

        /// <summary>
        /// 获取指定配置元素的元素键。
        /// </summary>
        /// <param name="element">要为其返回键的 System.Configuration.ConfigurationElement。</param>
        /// <returns>返回一个 System.Object，用作指定 System.Configuration.ConfigurationElement 的键。</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MapCollection)element).Name;
        }
        #endregion

        #region 属性
        /// <summary>
        ///    获取 System.Configuration.ConfigurationElementCollection 的类型。
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// 用于标识配置文件中此元素集合的名称
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        protected override string ElementName
        {
            get { return MapKey; }
        }

        /// <summary>
        /// 用于标识配置文件中此元素集合的名称。
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(NameKey, IsRequired = false)]
        public string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        /// <summary>
        /// 用于标识配置文件中引用其它元素的名称。
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(RefKey, IsRequired = false)]
        public string RefName
        {
            get { return (string)this[RefKey]; }
            set { this[RefKey] = value; }
        }

        /// <summary>
        /// 用于标识配置文件中引用提供者的名称
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(ProviderRefKey, IsRequired = false)]
        public string ProviderRefName
        {
            get { return (string)this[ProviderRefKey]; }
            set { this[ProviderRefKey] = value; }
        }

        /// <summary>
        /// 用于标识配置文件中此元素的值
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(ValueKey, IsRequired = false)]
        public string ValueName
        {
            get
            {
                return (string)this[ValueKey];
            }
            set { this[ValueKey] = value; }
        }

        /// <summary>
        /// 用于标识配置文件中此元素类型
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(TypeKey, DefaultValue = "System.String")]
        public string TypeName
        {
            get { return (string)this[TypeKey]; }
            set { this[TypeKey] = value; }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取MapCollection元素集合
        /// </summary>
        /// <returns>返回MapCollection元素集合</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        public object[] GetTypedParameterValues()
        {
            return this.Cast<MapCollection>()
                       .ToArray();
        }

        /// <summary>
        /// 获取子元素集合
        /// </summary>
        /// <returns>返回子元素集合</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        public object GetTypedPropertyValue()
        {
            var type = Type.GetType(TypeName, throwOnError: true);
            var maps = GetTypedParameterValues().OfType<MapCollection>();
            var mapCollections = maps as MapCollection[] ?? maps.ToArray();
            if (mapCollections.Any())
            {
                var results = new List<object>();
                foreach (var map in mapCollections)
                {
                    object items = map.GetTypedParameterValues().OfType<PropertyElement>().Select(p => p.GetTypedPropertyValue()).ToArray();
                    results.Add(new
                    {
                        Name = Convert.ChangeType(Name, typeof(string), CultureInfo.InvariantCulture),
                        Value = Convert.ChangeType(map.Name, type, CultureInfo.InvariantCulture),
                        Items = items
                    });
                }
                return results;
            }
            else if (!string.IsNullOrEmpty(ValueName))
            {
                    return new
                    {
                        Name = Convert.ChangeType(Name, typeof (string), CultureInfo.InvariantCulture),
                        Value = Convert.ChangeType(ValueName, type, CultureInfo.InvariantCulture),
                    };
            }
            else if (!string.IsNullOrEmpty(RefName))
                return Convert.ChangeType(RefName, type, CultureInfo.InvariantCulture);
            else
                return Convert.ChangeType(ProviderRefName, type, CultureInfo.InvariantCulture);
        }
        #endregion

    }
}
