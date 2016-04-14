using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.Cryptography
{

    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public sealed class CryptogramProvider
    {
        private static readonly string key = "lQa9_&skzly%!9fs2@*UNA($ck_^:)'aI9e.^2Lbx9,5lf!j+~Hz@^hakuJ^crOb";

        public static string Decrypt3DES(string sourceStr)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
            provider.Mode = CipherMode.ECB;
            ICryptoTransform transform = provider.CreateDecryptor();
            string str = "";
            try
            {
                byte[] inputBuffer = Convert.FromBase64String(sourceStr);
                str = Encoding.UTF8.GetString(transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch
            {
            }
            return str;
        }

        public static string Decrypt3DES(string sourceStr, string key)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
            provider.Mode = CipherMode.ECB;
            ICryptoTransform transform = provider.CreateDecryptor();
            string str = "";
            try
            {
                byte[] inputBuffer = Convert.FromBase64String(sourceStr);
                str = Encoding.UTF8.GetString(transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch
            {
            }
            return str;
        }

        public static string Decrypt3DES(string sourceStr, string key, Encoding encoding)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = new MD5CryptoServiceProvider().ComputeHash(encoding.GetBytes(key));
            provider.Mode = CipherMode.ECB;
            ICryptoTransform transform = provider.CreateDecryptor();
            string str = "";
            try
            {
                byte[] inputBuffer = Convert.FromBase64String(sourceStr);
                str = encoding.GetString(transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch
            {
            }
            return str;
        }

        public static string Decrypt3DESWithTrueKey(string sourceStr, string key)
        {
            ICryptoTransform transform = new TripleDESCryptoServiceProvider { Key = Encoding.UTF8.GetBytes(key), Mode = CipherMode.ECB }.CreateDecryptor();
            string str = "";
            try
            {
                byte[] inputBuffer = Convert.FromBase64String(sourceStr);
                str = Encoding.UTF8.GetString(transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch
            {
            }
            return str;
        }

        public static string DecryptBase64(string sourceStr)
        {
            byte[] bytes = Convert.FromBase64String(sourceStr);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string DecryptBase64(string sourceStr, Encoding encoding)
        {
            byte[] bytes = Convert.FromBase64String(sourceStr);
            return encoding.GetString(bytes);
        }

        public static string Encrypt3DES(string sourceStr)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
            provider.Mode = CipherMode.ECB;
            ICryptoTransform transform = provider.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(sourceStr);
            return Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        public static string Encrypt3DES(string sourceStr, string key)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
            provider.Mode = CipherMode.ECB;
            ICryptoTransform transform = provider.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(sourceStr);
            return Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        public static string Encrypt3DES(string sourceStr, string key, Encoding encoding)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = new MD5CryptoServiceProvider().ComputeHash(encoding.GetBytes(key));
            provider.Mode = CipherMode.ECB;
            ICryptoTransform transform = provider.CreateEncryptor();
            byte[] bytes = encoding.GetBytes(sourceStr);
            return Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        public static string Encrypt3DESWithTrueKey(string sourceStr, string key)
        {
            ICryptoTransform transform = new TripleDESCryptoServiceProvider { Key = Encoding.UTF8.GetBytes(key), Mode = CipherMode.ECB }.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(sourceStr);
            return Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        public static string EncryptBase64(string sourceStr)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(sourceStr));
        }

        public static string EncryptBase64(string sourceStr, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(sourceStr));
        }

        public static string MD5(string sourceStr)
        {
            string str = string.Empty;
            System.Security.Cryptography.MD5 md = System.Security.Cryptography.MD5.Create();
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);
            if (sourceStr == null)
            {
                sourceStr = string.Empty;
            }
            writer.Write(sourceStr);
            writer.Flush();
            stream.Seek(0L, SeekOrigin.Begin);
            byte[] buffer = md.ComputeHash(stream);
            stream.SetLength(0L);
            writer.Flush();
            for (int i = 0; i < buffer.Length; i++)
            {
                writer.Write("{0:X2}", buffer[i]);
            }
            writer.Flush();
            stream.Seek(0L, SeekOrigin.Begin);
            str = reader.ReadToEnd();
            writer.Close();
            writer.Dispose();
            reader.Close();
            reader.Dispose();
            stream.Close();
            stream.Dispose();
            return str;
        }

        public static string MD5Base64(string sourceStr)
        {
            byte[] bytes = new UnicodeEncoding().GetBytes(sourceStr);
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(provider.ComputeHash(bytes));
        }

        public static string SHA256(string sourceStr)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sourceStr);
            SHA256Managed managed = new SHA256Managed();
            byte[] inArray = managed.ComputeHash(bytes);
            managed.Clear();
            return Convert.ToBase64String(inArray);
        }
    }
}

