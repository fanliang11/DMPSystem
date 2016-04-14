/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/

using System.Runtime.Serialization;

namespace DMPSystem.Core.Common.ServicesException
{
    /// <summary>
    /// 错误信息类
    /// </summary>
     [DataContract]
    public partial class ErrorInfo
    {
        /// <summary>
        /// 错误内容
        /// </summary>
        [DataMember]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        [DataMember]
        public int ErrorCode
        {
            get;
            set;
        }
    }
}
