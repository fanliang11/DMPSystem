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
    /// RSA加密签名类
    /// </summary>
    public  class RSAEncryString
    {
        /// <summary>  
        /// RSA获取公钥私钥  
        /// </summary>  
        /// <param name="strPrivateKey">生成返回的私钥</param>  
        /// <param name="strPublicKey">生成返回的公钥</param>  
        public void RsaCreateKey(out string strPublicKey, out string strPrivateKey)
        {
            var RSA = new RSACryptoServiceProvider(512);
            strPublicKey = Convert.ToBase64String(RSA.ExportCspBlob(false));
            strPrivateKey = Convert.ToBase64String(RSA.ExportCspBlob(true));
        }


        /// <summary>  
        /// RSA加密  
        /// </summary>  
        /// <param name="source"></param>
        /// <param name="strPublicKey"></param>  
        /// <returns></returns>  
        public string RsaEncrypt(string source, string strPublicKey)
        {

            byte[] dataToDecrypt = Encoding.UTF8.GetBytes(source);
            try
            {
                var rsa = new RSACryptoServiceProvider();
                byte[] bytesPublicKey = Convert.FromBase64String(strPublicKey);
                rsa.ImportCspBlob(bytesPublicKey);
                //获取RSA最大加密的长度
                Int32 keySize = rsa.KeySize / 8;
                Int32 blockSize = keySize - 11;
                if (dataToDecrypt.Length > blockSize)
                {
                    //分配加密次数
                    Int32 encryptIndex = 0;
                    if (dataToDecrypt.Length % blockSize != 0)//不能整除
                    {
                        encryptIndex = dataToDecrypt.Length / blockSize + 1;
                    }
                    else
                    {
                        encryptIndex = dataToDecrypt.Length / blockSize;
                    }
                    //分配存储已加密的byte数组，请注意下这里分配的大小//每次只能加密117长度的数据，但是每次加密117长度的数据要生成128长度的密文。
                    var encryptByte = new Byte[encryptIndex * keySize];
                    for (Int32 i = 0; i < encryptIndex; i++)
                    {
                        var block = new Byte[blockSize];
                        if (i == (encryptIndex - 1))
                        {
                            //最后一次加密
                            Array.Copy(dataToDecrypt, i * blockSize, block, 0, dataToDecrypt.Length % blockSize);
                        }
                        else
                        {
                            Array.Copy(dataToDecrypt, i * blockSize, block, 0, blockSize);
                        }
                        var block1 = rsa.Encrypt(block, false);
                        //将加密过的Byte数组保存到EncryptByte中
                        Array.Copy(block1, 0, encryptByte, i * keySize, keySize);
                    }
                    return Convert.ToBase64String(encryptByte).TrimEnd();
                }
                byte[] b = rsa.Encrypt(dataToDecrypt, false);
                return Convert.ToBase64String(b).TrimEnd();
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>  
        /// RSA解密  
        /// </summary>  
        /// <param name="strRsa">需要解密的字符串</param>  
        /// <param name="strPrivateKey">私钥</param>  
        /// <returns>返回解密后的字符串</returns>  
        public string RsaDecrypt(string strRsa, string strPrivateKey)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(strRsa);
            try
            {
                Byte[] Block1;
                RSACryptoServiceProvider.UseMachineKeyStore = true;
                var rsa = new RSACryptoServiceProvider();
                byte[] bytesPublicKey = Convert.FromBase64String(strPrivateKey);
                rsa.ImportCspBlob(bytesPublicKey);
                Int32 keySize = rsa.KeySize / 8;
                Int32 blockSize = keySize - 11;
                if (dataToDecrypt.Length == keySize)
                {
                    byte[] bytesDecrypt = rsa.Decrypt(dataToDecrypt, false);
                    return Encoding.UTF8.GetString(bytesDecrypt, 0, bytesDecrypt.Length).TrimEnd();
                }
                // www.2cto.com 这个变量用来记录所有解密后的Byte数组的长度
                Int32 len = 0;
                //保存每次解密后的Byte数组
                var list = new List<Byte[]>();
                //计算解密需要的次数
                Int32 encryptIndex = dataToDecrypt.Length / keySize;

                for (Int32 i = 0; i < encryptIndex; i++)
                {
                    Byte[] block = new Byte[keySize];
                    Array.Copy(dataToDecrypt, i * keySize, block, 0, keySize);
                    Block1 = rsa.Decrypt(block, false);
                    len += Block1.Length;
                    list.Add(Block1);
                }
                Byte[] encryptByte = new Byte[len];
                for (Int32 i = 0; i < list.Count; i++)
                {
                    Array.Copy(list[i], 0, encryptByte, i * blockSize, blockSize);
                }
                return Encoding.UTF8.GetString(encryptByte, 0, len).Replace("\0","").TrimEnd();
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="strDataToSign">需要签名的数据。</param>
        /// <param name="strPrivateKey">私钥</param>
        /// <returns>返回签名字符串</returns>
        public string HashAndSign(string strDataToSign, string strPrivateKey)
        {
            var byteConverter = new ASCIIEncoding();
            byte[] dataToSign = byteConverter.GetBytes(strDataToSign);
            try
            {
                var rsAalg = new RSACryptoServiceProvider();
                rsAalg.ImportCspBlob(Convert.FromBase64String(strPrivateKey));
                byte[] signedData = rsAalg.SignData(dataToSign, new SHA1CryptoServiceProvider());
                string strSignedData = Convert.ToBase64String(signedData);
                return strSignedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 验证数据签名
        /// </summary>
        /// <param name="strDataToVerify"> 已签名的数据。</param>
        /// <param name="strSignedData">要验证的签名数据。</param>
        /// <param name="strPublicKey">公钥</param>
        /// <returns>如果签名有效，则为 true；否则为 false。</returns>
        public bool VerifySignedHash(string strDataToVerify, string strSignedData, string strPublicKey)
        {

            try
            {
                byte[] signedData = Convert.FromBase64String(strSignedData);
                ASCIIEncoding byteConverter = new ASCIIEncoding();
                byte[] DataToVerify = byteConverter.GetBytes(strDataToVerify);
                RSACryptoServiceProvider rsAalg = new RSACryptoServiceProvider();
                rsAalg.ImportCspBlob(Convert.FromBase64String(strPublicKey));
                return rsAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), signedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }
    }
}
