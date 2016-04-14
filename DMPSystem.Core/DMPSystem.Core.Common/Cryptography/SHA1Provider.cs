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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.Cryptography
{
    /// <summary>
    ///  sha1 加密算法类
    /// </summary>
    public class SHA1Provider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encypStr">需要加密的字符串</param>
        /// <param name="charset">编码</param>
        /// <param name="type">位长度</param>
        /// <returns>返回加密后的字符串</returns>
        public static String Sha1(string encypStr, string charset = "gb2312", BitType type = BitType.x16)
        {
            try
            {
                string retStr;
                if (type == BitType.x16)
                {
                    System.Security.Cryptography.SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                    retStr = BitConverter.ToString(sha.ComputeHash(System.Text.Encoding.GetEncoding(charset).GetBytes(encypStr)), 4, 8);
                    retStr = retStr.Replace("-", "");
                }
                else
                {
                    System.Security.Cryptography.SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                    retStr = BitConverter.ToString(sha.ComputeHash(System.Text.Encoding.GetEncoding(charset).GetBytes(encypStr)));
                    retStr = retStr.Replace("-", "");
                }
                return retStr;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return null;
            }
        }
    }
}
