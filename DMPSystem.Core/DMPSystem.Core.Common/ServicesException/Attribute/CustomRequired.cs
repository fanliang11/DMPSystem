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
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.ServicesException.Attribute
{
    /// <summary>
    /// 验证对象是否为null自定义特性
    /// </summary>
    public class CustomRequired : ValidationAttribute
    {
        /// <summary>
        /// 错误号
        /// </summary>
        public object Code { get; set; }

        /// <summary>
        ///   确定对象的指定值是否有效。
        /// </summary>
        /// <param name="value">要验证的对象的值。</param>
        /// <returns> 如果指定的值有效，则为 true；否则，为 false。</returns>
        public override bool IsValid(object value)
        {
            return !string.Equals(value, string.Empty) && !string.Equals(value, null);
        }

        /// <summary>
        ///   根据当前的验证特性来验证指定的值。
        /// </summary>
        /// <param name="value">要验证的值。</param>
        /// <param name="validationContext"> 有关验证操作的上下文信息。</param>
        /// <returns>  System.ComponentModel.DataAnnotations.ValidationResult 类的实例。</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = base.ErrorMessage;
            var isSuccess = false;
            if (value != null)
            {
                if (!string.Equals(value, string.Empty))
                {
                    isSuccess = true;
                }
            }
           if(!isSuccess)
           {
               if (string.IsNullOrEmpty(ErrorMessage))
               {
                   return
                       new ValidationResult(
                           string.Format("{0}字段非法,错误号{1}", validationContext.DisplayName,
                                         (int) Enum.Parse(Code.GetType(), Code.ToString())),
                           new[] {validationContext.MemberName});
               }
               else
               {

                   return
                       new ValidationResult(
                           string.Format("{0},错误号{1}", string.Format(ErrorMessage, validationContext.DisplayName),
                                         (int) Enum.Parse(Code.GetType(), Code.ToString())),
                           new[] {validationContext.MemberName});
               }
           }
            return ValidationResult.Success;

        }
    }
}
