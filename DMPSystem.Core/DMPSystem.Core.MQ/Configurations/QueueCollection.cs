using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Configurations
{
    /// <summary>
    /// Queue节点集合的配置元素。
    /// </summary>
    /// <remarks>
    /// 	<para>创建：范亮</para>
    /// 	<para>日期：2016/4/2</para>
    /// </remarks>
    public sealed class QueueCollection : ConfigurationElementCollection
    {
        #region 字段
        private const string ParameterKey = "property";
        private const string InitMethodKey = "init-method";
        private const string IdKey = "id";
        private const string ClassKey = "class";
        #endregion

        #region 属性
        /// <summary>
        /// 用于标识配置文件中此元素集合的提供者对象。
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(ClassKey, IsRequired = true)]
        public string ClassName
        {
            get { return (string)this[ClassKey]; }
            set { this[ClassKey] = value; }
        }

        /// <summary>
        /// 用于标识配置文件中此元素集合的名称。
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(IdKey, IsRequired = true)]
        public string IdName
        {
            get { return (string)this[IdKey]; }
            set { this[IdKey] = value; }
        }

        /// <summary>
        /// 用于标识提供者对象初始化的方法
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(InitMethodKey, IsRequired = false)]
        public string InitMethodName
        {
            get { return (string)this[InitMethodKey]; }
            set { this[InitMethodKey] = value; }
        }
        #endregion

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
            return ((PropertyElement)element).Name;
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
        /// 获取子元素类型集合
        /// </summary>
        /// <returns>返回子元素类型集合</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        public object[] GetTypedPropertyValues()
        {
            return this.Cast<PropertyElement>()
                       .ToArray();
        }

        /// <summary>
        /// 获取提供者对象的类型
        /// </summary>
        /// <returns>返回提供者对象的类型</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        public Type GetFactoryType()
        {
            return Type.GetType(ClassName, throwOnError: true);
        }

       /// <summary>
        /// 创建新的ParameterElement元素
       /// </summary>
        /// <returns>返回ParameterElement元素</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        internal ParameterElement NewElement()
        {
            var element = CreateNewElement();
            base.BaseAdd(element);
            return (ParameterElement)element;
        }
    }
}
