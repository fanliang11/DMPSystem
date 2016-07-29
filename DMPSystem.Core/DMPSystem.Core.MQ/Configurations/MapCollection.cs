using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Configurations
{
    /// <summary>
    /// Map节点集合的配置元素。
    /// </summary>
    /// <remarks>
    /// 	<para>创建：范亮</para>
    /// 	<para>日期：2016/4/2</para>
    /// </remarks>
    public sealed class MapCollection
    : ConfigurationElementCollection
    {
        #region 字段
        private const string ParameterKey = "property";
        private const string NameKey = "name";
        #endregion

        #region 友元方法
        /// <summary>
        ///    创建<see cref="PropertyElement"/> 元素
        /// </summary>
        /// <returns>返回一个新的 System.Configuration.ConfigurationElement。</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        protected override ConfigurationElement CreateNewElement()
        {
            var element = new PropertyElement();
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
            return ((PropertyElement)element).ValueName;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 用于标识配置文件中此元素集合的名称。
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(NameKey, IsRequired = false)]
        public string  Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
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
            get { return ParameterKey; }
        }

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
        #endregion

        #region 公共方法
        /// <summary>
        ///  获取子元素类型集合
        /// </summary>
        /// <returns>返回子元素类型集合</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        public object[] GetTypedParameterValues()
        {
            return this.Cast<PropertyElement>()
                       .ToArray();
        }
        #endregion
    }
}
