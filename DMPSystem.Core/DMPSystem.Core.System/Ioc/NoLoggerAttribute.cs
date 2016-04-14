/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.System.Ioc
{
    /// <summary>
    /// 设置不产生日志的特性类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public class NoLoggerAttribute : Attribute
    {
        #region 字段
        private bool _disable;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化一个新的<c>InterceptMethodAttribute</c>类型。
        /// </summary>
        /// <param name="disable">缓存方式。</param>
        public NoLoggerAttribute(bool disable)
        {
            this._disable = disable;
        }

        /// <summary>
        /// 初始化一个新的<c>InterceptMethodAttribute</c>类型。
        /// </summary>
        public NoLoggerAttribute()
            : this(true)
        {

        }
        #endregion

        #region 公共属性
       /// <summary>
       /// 是否禁用日志
       /// </summary>
        public bool Disable
        {
            get
            {
                return _disable;
            }
            set
            {
                _disable = value;
            }
        }
        #endregion
    }

}



