using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.Extensions
{
   public static class StringExtension
    {
        private const string DataFormat = "ddHHmmss";

        public static string GetTradeNo(this string orderNumber, DateTime timeStamp)
        {
            return string.Format("{0}{1}", orderNumber, timeStamp.ToString(DataFormat));
        }

        public static string GetOrderNumberFormat(this string orderNumber)
        {
            if (orderNumber.Length > DataFormat.Length)
                return orderNumber.Substring(0, orderNumber.Length - DataFormat.Length);
            else
                return orderNumber;
        }
    }
}
