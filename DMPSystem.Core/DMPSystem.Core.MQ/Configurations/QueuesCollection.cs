using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.EventBus.Configurations
{
    /// <summary>
    /// Queues节点集合的配置元素。
    /// </summary>
    /// <remarks>
    /// 	<para>创建：范亮</para>
    /// 	<para>日期：2016/4/2</para>
    /// </remarks>
    public sealed class QueuesCollection : ConfigurationElementCollection
    {
        #region 字段
        private const string ParameterKey = "queue";
        private int _nextKey;
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
            get { return ParameterKey; }
        }
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
            var element = new QueueCollection();
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
            return ((QueueCollection)element).IdName;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取子元素类型集合
        /// </summary>
        /// <returns>返回子元素类型集合</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        public object[] GetTypedParameterValues()
        {
            return this.Cast<QueueCollection>()
                       .ToArray();
        }
        #endregion

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
