using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DMPSystem.Core.Common.Providers
{
   public  class IpProvider
    {
        public static string DigitalIPIntoDottedIP(long ip)
        {
            string str2 = ip.ToString("X");
            while (str2.Length < 8)
            {
                str2 = "0" + str2;
            }
            string s = str2.Substring(6, 2);
            string str4 = str2.Substring(4, 2);
            string str5 = str2.Substring(2, 2);
            string str6 = str2.Substring(0, 2);
            string str7 = int.Parse(s, NumberStyles.HexNumber).ToString();
            string str8 = int.Parse(str4, NumberStyles.HexNumber).ToString();
            string str9 = int.Parse(str5, NumberStyles.HexNumber).ToString();
            string str10 = int.Parse(str6, NumberStyles.HexNumber).ToString();
            switch (str7.Length)
            {
                case 1:
                    str7 = "00" + str7;
                    break;

                case 2:
                    str7 = "0" + str7;
                    break;
            }
            switch (str8.Length)
            {
                case 1:
                    str8 = "00" + str8;
                    break;

                case 2:
                    str8 = "0" + str8;
                    break;
            }
            switch (str9.Length)
            {
                case 1:
                    str9 = "00" + str9;
                    break;

                case 2:
                    str9 = "0" + str9;
                    break;
            }
            switch (str10.Length)
            {
                case 1:
                    str10 = "00" + str10;
                    break;

                case 2:
                    str10 = "0" + str10;
                    break;
            }
            return (str10 + "." + str9 + "." + str8 + "." + str7);
        }

        public static long DottedIPIntoDigitalIP(string ip)
        {
            long num = 0L;
            try
            {
                string[] strArray = ip.Split(new char[] { '.' });
                string s = "";
                foreach (string str2 in strArray)
                {
                    s = s + Convert.ToByte(str2).ToString("x").PadLeft(2, '0');
                }
                num = long.Parse(s, NumberStyles.HexNumber);
            }
            catch
            {
            }
            return num;
        }

        public static long GetClientDigitalIP()
        {
            return DottedIPIntoDigitalIP(GetClientIPAddress());
        }

        public static string GetClientIPAddress()
        {
            ClientIPVariable variable;
            return GetClientIPAddress(out variable);
        }

        private static string GetClientIPAddress(out ClientIPVariable ipVar)
        {
            string str = null;
            ipVar = ClientIPVariable.None;
            if (string.Compare(HttpContext.Current.Request.Url.Scheme, "https", true) != 0)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    ipVar = ClientIPVariable.Http_X_Forwarded_For;
                    str = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(",;".ToCharArray())[0];
                }
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]))
                {
                    ipVar = ClientIPVariable.Http_Client_IP;
                    str = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                }
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["X_CLIENT_IP"]))
                {
                    ipVar = ClientIPVariable.X_Client_IP;
                    str = HttpContext.Current.Request.ServerVariables["X_CLIENT_IP"];
                }
            }
            if ((str == null) && !string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]))
            {
                ipVar = ClientIPVariable.Remote_Addr;
                str = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return str;
        }

        public static string GetRemoteIPAddress()
        {
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (addressList.Length > 0)
            {
                return addressList[0].ToString();
            }
            return "127.0.0.1";
        }

        public static bool IsIPLegality(string ip)
        {
            if (ValidationProvider.IsNullOrEmpty(ip))
            {
                return false;
            }
            ip = ip.Trim();
            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            return RegexProvider.IsMatch(ip, pattern);
        }

        private enum ClientIPVariable
        {
            None,
            Remote_Addr,
            Http_X_Forwarded_For,
            Http_Client_IP,
            X_Client_IP
        }
    }
}
