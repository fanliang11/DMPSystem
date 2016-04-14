using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DMPSystem.Core.Common.Providers
{
    public class CommonProvider
    {
        public static string FormatHtmlTag(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static int GetIndexInArray(string searchStr, string[] arrStr)
        {
            return GetIndexInArray(searchStr, arrStr, true);
        }

        public static int GetIndexInArray(string searchStr, string[] arrStr, bool caseInsensetive)
        {
            int num = -1;
            if (!string.IsNullOrEmpty(searchStr) && (arrStr.Length > 0))
            {
                for (int i = 0; i < arrStr.Length; i++)
                {
                    if (caseInsensetive)
                    {
                        if (searchStr == arrStr[i])
                        {
                            num = i;
                        }
                    }
                    else if (searchStr.ToLower() == arrStr[i].ToLower())
                    {
                        num = i;
                    }
                }
            }
            return num;
        }

        public static int GetLength(string str)
        {
            if (ValidationProvider.IsNullOrEmpty(str))
            {
                return 0;
            }
            return Encoding.UTF8.GetByteCount(str);
        }

        public static string[] Split(string sourceStr, string splitStr)
        {
            if (string.IsNullOrEmpty(sourceStr))
            {
                return new string[0];
            }
            if (sourceStr.IndexOf(splitStr) < 0)
            {
                return new string[] { sourceStr };
            }
            return Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);
        }

        public static string[] Split(string sourceStr, string splitStr, int count)
        {
            string[] strArray = new string[count];
            string[] strArray2 = Split(sourceStr, splitStr);
            for (int i = 0; i < count; i++)
            {
                if (i < strArray2.Length)
                {
                    strArray[i] = strArray2[i];
                }
                else
                {
                    strArray[i] = string.Empty;
                }
            }
            return strArray;
        }

        public static string Substring(string str, int length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            if (bytes.Length > length)
            {
                return Encoding.UTF8.GetString(bytes, 0, length);
            }
            return str;
        }

        public static string Substring(string str, int startIndex, int length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length *= -1;
                    if ((startIndex - length) < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex -= length;
                    }
                }
                if (startIndex > bytes.Length)
                {
                    return "";
                }
            }
            else if ((length >= 0) && ((length + startIndex) > 0))
            {
                length += startIndex;
                startIndex = 0;
            }
            else
            {
                return "";
            }
            if ((bytes.Length - startIndex) < length)
            {
                length = bytes.Length - startIndex;
            }
            return Encoding.UTF8.GetString(bytes, startIndex, length);
        }

        public static string UnFormatHtmlTag(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }
    }
}

