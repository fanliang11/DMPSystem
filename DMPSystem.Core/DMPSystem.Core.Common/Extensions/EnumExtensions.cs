/*----------------------------------------------------
 * 作者:范  亮
 * 创建时间：2015-11-28
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 范  亮      2015-11-28      创建
 ----------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.Extensions
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///  获取对枚举的描述信息
        /// </summary>
        /// <param name="value">枚举</param>
        /// <returns>返回枚举的描述信息</returns>
        public static string GetDisplay(this Enum value)
        {
            var attr = value.GetAttribute<DisplayAttribute>();
            return attr == null ? "" : attr.Name;
        }
        
        /// <summary>
        /// 获取枚举的自定义属性
        /// </summary>
        /// <typeparam name="T">自定义属性类型</typeparam>
        /// <param name="value">枚举</param>
        /// <returns>返回自定义属性</returns>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return Attribute.GetCustomAttribute(field, typeof(T)) as T;
        }

        /// <summary>
        /// 获取枚举的值
        /// </summary>
        /// <param name="value">枚举</param>
        /// <returns>返回枚举的值</returns>
        public static int GetValue(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// 获取枚举所有的值对象
        /// </summary>
        /// <param name="type">枚举</param>
        /// <returns>返回枚举的值对象</returns>
        public static List<Tuple<string, string>> GetEnumSource(this Type type)
        {
            if (!type.IsEnum)
                throw new Exception("type 类型必须为枚举类型!");

            var list = new List<Tuple<string, string>>();

            foreach (var value in Enum.GetValues(type))
            {
                var fieldName = Enum.GetName(type, value);
                var field = type.GetField(fieldName);
                var display = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
                if (display != null)
                    list.Add(new Tuple<string, string>(Convert.ToInt32(value) + "", display.Name));
                else
                    list.Add(new Tuple<string, string>(Convert.ToInt32(value) + "", fieldName));
            }
            return list;
        }
    }
}
