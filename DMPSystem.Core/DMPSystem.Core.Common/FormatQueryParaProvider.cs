/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/

using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common
{
    /// <summary>
    /// 格式化请求参数
    /// </summary>
    public class FormatQueryParaProvider
    {
        /// <summary>
        /// 模型实体转化请求参数 
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="model">模型</param>
        /// <param name="isOrder">是否排序</param>
        /// <returns>返回请求参数，格式:name=value&name=value</returns>
        public static string FormatQueryParaMap<T>(T model, bool isOrder)
        {
            StringBuilder para = new StringBuilder();
            try
            {
                var properties = typeof(T).GetProperties();
                if (isOrder)
                {
                    properties = (from prop in properties orderby prop.Name select prop).ToArray();
                }
                foreach (var property in properties)
                {
                    var attr = property.GetCustomAttribute<JsonPropertyAttribute>();
                    var propertyValue = property.GetValue(model) ?? "";
                    if (attr != null)
                    {
                        if (!string.IsNullOrEmpty(propertyValue.ToString()))
                        {
                            para.AppendFormat("{0}={1}&", attr.PropertyName, propertyValue);
                        }
                    }
                }
                if (para.Length == 0 == false)
                {
                    para.Length = para.Length - 1;
                }
                return para.ToString();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 模型实体转化请求参数 
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="model">模型</param>
        /// <param name="urlencode">是否url编码</param>
        /// <param name="isOrder">是否排序</param>
        /// <param name="upper">是否转为大写 </param>
        /// <returns>返回请求参数，格式:name=value&name=value</returns>
        public static string FormatQueryParaMap<T>(T model, bool urlencode, bool isOrder, bool upper = true)
        {
            StringBuilder para = new StringBuilder();
            try
            {
                var properties = typeof(T).GetProperties();
                if (isOrder)
                {
                    properties = (from prop in properties orderby prop.Name select prop).ToArray();
                }
                foreach (var property in properties)
                {
                    var attr = property.GetCustomAttribute<JsonPropertyAttribute>();
                    var propertyValue = property.GetValue(model) ?? "";
                    if (urlencode)
                    {
                        propertyValue = System.Web.HttpUtility.UrlEncode(propertyValue.ToString());
                    }
                    if (attr != null)
                    {
                        if (!string.IsNullOrEmpty(propertyValue.ToString()))
                        {
                            para.AppendFormat("{0}={1}&", attr.PropertyName, propertyValue);
                        }
                    }
                }
                if (para.Length == 0 == false)
                {
                    para.Length = para.Length - 1;
                }
                if(!upper)
                {
                    return para.ToString();
                }
                return para.ToString().ToUpper();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return null;
            }
        }
    }
}
