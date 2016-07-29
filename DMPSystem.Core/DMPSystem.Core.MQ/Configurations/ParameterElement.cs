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
    /// Parameter节点集合的配置元素。
    /// </summary>
    public sealed class ParameterElement : ConfigurationElement
    {
        #region 字段
        private const string ValueKey = "value";
        private const string TypeKey = "type";
        #endregion

        #region 构造函数
        /// <summary>
        /// 通过KEY键初始化构造<see cref="ParameterElement"/>
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        public ParameterElement(int key)
        {
            Key = key;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 用于标识元素
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        internal int Key { get; private set; }

        /// <summary>
        /// 用于标识元素对应的值
        /// </summary>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        [ConfigurationProperty(ValueKey, IsRequired = true)]
        public string ValueString
        {
            get { return (string)this[ValueKey]; }
            set { this[ValueKey] = value; }
        }

        /// <summary>
        /// 用于标识此元素类型
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

        #region  公共方法
        /// <summary>
        ///  获取子元素类型集合
        /// </summary>
        /// <returns>返回子元素类型集合</returns>
        /// <remarks>
        /// 	<para>创建：范亮</para>
        /// 	<para>日期：2016/4/2</para>
        /// </remarks>
        public object GetTypedParameterValue()
        {
            var type = Type.GetType(TypeName, throwOnError: true);
            return Convert.ChangeType(ValueString, type, CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
