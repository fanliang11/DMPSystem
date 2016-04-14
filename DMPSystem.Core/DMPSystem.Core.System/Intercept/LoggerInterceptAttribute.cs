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

namespace DMPSystem.Core.System.Intercept
{
    /// <summary>
    /// 设置判断日志拦截方法的特性类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public class LoggerInterceptAttribute : Attribute
    {
        #region 字段
        private string _message;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化一个新的<c>InterceptMethodAttribute</c>类型。
        /// </summary>
        /// <param name="method">缓存方式。</param>
        public LoggerInterceptAttribute(string message)
        {
            this._message = message;
        }

       /// <summary>
        ///  初始化一个新的<c>InterceptMethodAttribute</c>类型。
       /// </summary>
        public LoggerInterceptAttribute()
            : this(null)
        {

        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }
        #endregion
    }
}
