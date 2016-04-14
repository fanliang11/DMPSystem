using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.Providers
{
    public sealed class ConvertProvider
    {
        public static int BytesToInt32(byte[] data)
        {
            if (data.Length < 4)
            {
                return 0;
            }
            int num = 0;
            if (data.Length >= 4)
            {
                byte[] dst = new byte[4];
                Buffer.BlockCopy(data, 0, dst, 0, 4);
                num = BitConverter.ToInt32(dst, 0);
            }
            return num;
        }

        public static string BytesToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                string str = Convert.ToString(Convert.ToInt32(value, from), to);
                if (to == 2)
                {
                    switch (str.Length)
                    {
                        case 3:
                            str = "00000" + str;
                            break;

                        case 4:
                            str = "0000" + str;
                            break;

                        case 5:
                            str = "000" + str;
                            break;

                        case 6:
                            str = "00" + str;
                            break;

                        case 7:
                            str = "0" + str;
                            break;
                    }
                }
                return str;
            }
            catch
            {
                return "0";
            }
        }

        public static T ConvertByteToObject<T>(byte[] bytes) where T: class
        {
            if (bytes == null)
            {
                return default(T);
            }
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return (formatter.Deserialize(stream) as T);
            }
        }

        public static T ConvertTo<T>(object data)
        {
            if (ValidationProvider.IsNullOrEmpty(data))
            {
                return default(T);
            }
            try
            {
                if (data is T)
                {
                    return (T) data;
                }
                if (typeof(T).BaseType == typeof(Enum))
                {
                    return EnumProvider.GetInstance<T>(data);
                }
                if (data is IConvertible)
                {
                    return (T) Convert.ChangeType(data, typeof(T));
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        public static object ConvertTo(object data, Type targetType)
        {
            if (ValidationProvider.IsNullOrEmpty(data))
            {
                return null;
            }
            try
            {
                if (data is IConvertible)
                {
                    return Convert.ChangeType(data, targetType);
                }
                return data;
            }
            catch
            {
                return null;
            }
        }

        public static byte[] ConvertToByte(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0L;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        public static string ConvertToHalfStr(string str)
        {
            string str2 = str;
            string str3 = "１２３４５６７８９０ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ";
            string str4 = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            if (!string.IsNullOrEmpty(str))
            {
                str2 = null;
                foreach (char ch in str)
                {
                    int index = str3.IndexOf(ch);
                    if (index < 0)
                    {
                        str2 = str2 + ch.ToString();
                    }
                    else
                    {
                        str2 = str2 + str4[index].ToString();
                    }
                }
            }
            return str2;
        }

        public static string RepairZero(string text, int limitedLength)
        {
            string str = "";
            for (int i = 0; i < (limitedLength - text.Length); i++)
            {
                str = str + "0";
            }
            return (str + text);
        }

        public static byte[] StringToBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        public static bool ToBoolean(object data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty(data))
                {
                    return false;
                }
                return Convert.ToBoolean(data);
            }
            catch
            {
                return false;
            }
        }

        public static bool ToBoolean<T>(T data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<T>(data))
                {
                    return false;
                }
                return Convert.ToBoolean(data);
            }
            catch
            {
                return false;
            }
        }

        public static DateTime ToDateTime(object date)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty(date))
                {
                    return Convert.ToDateTime("1900-1-1");
                }
                return Convert.ToDateTime(date);
            }
            catch
            {
                return Convert.ToDateTime("1900-1-1");
            }
        }

        public static decimal ToDecimal(object data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty(data))
                {
                    return 0M;
                }
                return Convert.ToDecimal(data);
            }
            catch
            {
                return 0M;
            }
        }

        public static decimal ToDecimal<T>(T data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<T>(data))
                {
                    return 0M;
                }
                return Convert.ToDecimal(data);
            }
            catch
            {
                return 0M;
            }
        }

        public static decimal ToDecimal(object data, int decimals)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<object>(data))
                {
                    return 0M;
                }
                return Math.Round(Convert.ToDecimal(data), decimals);
            }
            catch
            {
                return 0M;
            }
        }

        public static decimal ToDecimal<T>(T data, int decimals)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<T>(data))
                {
                    return 0M;
                }
                return Math.Round(Convert.ToDecimal(data), decimals);
            }
            catch
            {
                return 0M;
            }
        }

        public static double ToDouble(object data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty(data))
                {
                    return 0.0;
                }
                return Convert.ToDouble(data);
            }
            catch
            {
                return 0.0;
            }
        }

        public static double ToDouble<T>(T data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<T>(data))
                {
                    return 0.0;
                }
                return Convert.ToDouble(data);
            }
            catch
            {
                return 0.0;
            }
        }

        public static double ToDouble(object data, int decimals)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<object>(data))
                {
                    return 0.0;
                }
                return Math.Round(Convert.ToDouble(data), decimals);
            }
            catch
            {
                return 0.0;
            }
        }

        public static double ToDouble<T>(T data, int decimals)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<T>(data))
                {
                    return 0.0;
                }
                return Math.Round(Convert.ToDouble(data), decimals);
            }
            catch
            {
                return 0.0;
            }
        }

        public static double ToDouble<T>(T data, int decimals, MidpointRounding pointRound)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<T>(data))
                {
                    return 0.0;
                }
                return Math.Round(Convert.ToDouble(data), decimals, pointRound);
            }
            catch
            {
                return 0.0;
            }
        }

        public static float ToFloat(object data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<object>(data))
                {
                    return 0f;
                }
                return Convert.ToSingle(data);
            }
            catch
            {
                return 0f;
            }
        }

        public static float ToFloat<T>(T data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<T>(data))
                {
                    return 0f;
                }
                return Convert.ToSingle(data);
            }
            catch
            {
                return 0f;
            }
        }

        public static Guid ToGuid(object data)
        {
            if (ValidationProvider.IsNullOrEmpty(data))
            {
                return Guid.Empty;
            }
            try
            {
                return new Guid(data.ToString());
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public static int ToInt32(object data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty(data))
                {
                    return 0;
                }
                return Convert.ToInt32(data);
            }
            catch
            {
                return 0;
            }
        }

        public static int ToInt32<T>(T data)
        {
            try
            {
                if (ValidationProvider.IsNullOrEmpty<T>(data))
                {
                    return 0;
                }
                return Convert.ToInt32(data);
            }
            catch
            {
                return 0;
            }
        }

        public static string ToString(object data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            return data.ToString();
        }
    }
}
