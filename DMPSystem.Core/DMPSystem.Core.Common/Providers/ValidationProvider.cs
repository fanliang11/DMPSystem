using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.Providers
{
   public  class ValidationProvider
    {
        public static bool IsBjChar(char ctr)
        {
            int num = ctr;
            return ((num >= 0x20) && (num <= 0x7e));
        }

        public static bool IsDate(ref string date)
        {
            if (IsNullOrEmpty((string) date))
            {
                return true;
            }
            date = date.Trim();
            date = date.Replace(@"\", "-");
            date = date.Replace("/", "-");
            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }
            try
            {
                date = Convert.ToDateTime((string) date).ToString("d");
                return true;
            }
            catch
            {
                if (IsInt(date))
                {
                    if (date.Length == 8)
                    {
                        string str = date.Substring(0, 4);
                        string str2 = date.Substring(4, 2);
                        string str3 = date.Substring(6, 2);
                        if ((Convert.ToInt32(str) < 0x76c) || (Convert.ToInt32(str) > 0x834))
                        {
                            return false;
                        }
                        if ((Convert.ToInt32(str2) > 12) || (Convert.ToInt32(str3) > 0x1f))
                        {
                            return false;
                        }
                        date = Convert.ToDateTime(str + "-" + str2 + "-" + str3).ToString("d");
                        return true;
                    }
                    if (date.Length == 6)
                    {
                        string str4 = date.Substring(0, 4);
                        string str5 = date.Substring(4, 2);
                        if ((Convert.ToInt32(str4) < 0x76c) || (Convert.ToInt32(str4) > 0x834))
                        {
                            return false;
                        }
                        if (Convert.ToInt32(str5) > 12)
                        {
                            return false;
                        }
                        date = Convert.ToDateTime(str4 + "-" + str5).ToString("d");
                        return true;
                    }
                    if (date.Length == 5)
                    {
                        string str6 = date.Substring(0, 4);
                        string str7 = date.Substring(4, 1);
                        if ((Convert.ToInt32(str6) < 0x76c) || (Convert.ToInt32(str6) > 0x834))
                        {
                            return false;
                        }
                        date = str6 + "-" + str7;
                        return true;
                    }
                    if (date.Length == 4)
                    {
                        string str8 = date.Substring(0, 4);
                        if ((Convert.ToInt32(str8) < 0x76c) || (Convert.ToInt32(str8) > 0x834))
                        {
                            return false;
                        }
                        date = Convert.ToDateTime(str8).ToString("d");
                        return true;
                    }
                }
                return false;
            }
        }

        public static bool IsEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            email = email.Trim();
            string pattern = @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsExceedMaxLength(string str, int maxLength)
        {
            return (CommonProvider.GetLength(str) >= maxLength);
        }

        public static bool IsIdCard(string idCard)
        {
            if (IsNullOrEmpty(idCard))
            {
                return true;
            }
            idCard = idCard.Trim();
            StringBuilder builder = new StringBuilder();
            builder.Append("^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
            builder.Append("50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
            builder.Append(@"(\d{13}|\d{15}[\dx])$");
            return RegexProvider.IsMatch(idCard, builder.ToString());
        }

        public static bool IsInArray(string searchStr, string[] arrStr)
        {
            return IsInArray(searchStr, arrStr, true);
        }

        public static bool IsInArray(string searchStr, string arrStr)
        {
            return IsInArray(searchStr, CommonProvider.Split(arrStr, ","), false);
        }

        public static bool IsInArray(string searchStr, string[] arrStr, bool caseInsensetive)
        {
            return (CommonProvider.GetIndexInArray(searchStr, arrStr, caseInsensetive) >= 0);
        }

        public static bool IsInArray(string searchStr, string arrStr, string splitStr)
        {
            return IsInArray(searchStr, CommonProvider.Split(arrStr, splitStr), true);
        }

        public static bool IsInArray(string searchStr, string arrStr, string splitStr, bool caseInsensetive)
        {
            return IsInArray(searchStr, CommonProvider.Split(arrStr, splitStr), caseInsensetive);
        }

        public static bool IsInt(string number)
        {
            if (IsNullOrEmpty(number))
            {
                return false;
            }
            number = number.Trim();
            string pattern = "^[1-9]+[0-9]*$";
            return RegexProvider.IsMatch(number, pattern);
        }

        public static bool IsIPLegality(string ip)
        {
            return IpProvider.IsIPLegality(ip);
        }

        public static bool IsLengthRange(string str, int minLength, int maxLength)
        {
            int length = CommonProvider.GetLength(str);
            return ((length >= minLength) && (length < maxLength));
        }

        public static bool IsMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return false;
            }
            mobile = mobile.Trim();
            string pattern = @"^((\(\d{3}\))|(\d{3}\-))?(13|15|18|14|17)\d{9}$";
            return Regex.IsMatch(mobile, pattern);
        }

        public static bool IsNullOrEmpty(object data)
        {
            return IsNullOrEmpty<object>(data);
        }

        public static bool IsNullOrEmpty(string text)
        {
            return ((text == null) || string.IsNullOrEmpty(text.ToString().Trim()));
        }

        public static bool IsNullOrEmpty<T>(T data)
        {
            if (data == null)
            {
                return true;
            }
            if (data.GetType() == typeof(string))
            {
                return string.IsNullOrEmpty(data.ToString().Trim());
            }
            return (data.GetType() == typeof(DBNull));
        }

        public static bool IsNumber(string number)
        {
            if (IsNullOrEmpty(number))
            {
                return false;
            }
            number = number.Trim();
            string pattern = "^[-]?[0-9]*[.]?[0-9]*$";
            return RegexProvider.IsMatch(number, pattern);
        }

        public static bool IsOdd(int n)
        {
            return ((n % 2) != 0);
        }

        public static bool IsPhone(string phone)
        {
            if (IsNullOrEmpty(phone))
            {
                return false;
            }
            phone = phone.Trim();
            string pattern = @"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$";
            return RegexProvider.IsMatch(phone, pattern);
        }

        public static bool IsQjChar(char ctr)
        {
            if (ctr == '　')
            {
                return true;
            }
            int num = ctr - 0xfee0;
            if (num < 0x20)
            {
                return false;
            }
            return IsBjChar((char) num);
        }

        public static bool IsValidInput(ref string input)
        {
            try
            {
                if (!IsNullOrEmpty((string) input))
                {
                    input = input.Replace("'", "''").Trim();
                    string str = "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
                    foreach (string str2 in str.Split(new char[] { '|' }))
                    {
                        if (input.ToLower().IndexOf(str2) != -1)
                        {
                            input = string.Empty;
                            return false;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool NotHasInvalidChar(string input)
        {
            if (IsNullOrEmpty(input))
            {
                return false;
            }
            input = input.Trim();
            string pattern = "^[^<>'~`\x00b7!@#$%^&*()]+$";
            return RegexProvider.IsMatch(input, pattern);
        }
    }
}

