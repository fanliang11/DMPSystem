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

namespace DMPSystem.Core.Common
{
    /// <summary>
    /// 获取随机数
    /// </summary>
    public class RandomProvider
    {
        /// <summary>  
        /// 根据GUID获取16位的唯一字符串  
        /// </summary>  
        /// <returns>返回16位的唯一字符串 </returns>  
        public static string GuidTo16String()
        {
            long i = 1;
            var guidByte=  Guid.NewGuid().ToByteArray();
            foreach (byte b in guidByte)
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

    }
}
