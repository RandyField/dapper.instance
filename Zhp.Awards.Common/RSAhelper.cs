using Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Zhp.Awards.Common
{
    public class RSAhelper
    {
        /// <summary>
        /// RSA_KEY大小
        /// </summary>
        private const int RSA_KEY_SIZE = 2048;

        /// <summary>
        /// RSA公钥文件名
        /// </summary>
        private const string PUBLIC_KEY_FILE_NAME = "RSA.Pub";

        /// <summary>
        /// RSA私钥文件名
        /// </summary>
        private const string PRIVATE_KEY_FILE_NAME = "RSA.Private";

        /// <summary>
        /// 路径
        /// </summary>
        private const string PATH = @"F:\\RSA";

        /// <summary>
        /// 在给定路径中生成XML格式的私钥和公钥。
        /// </summary>
        public void GenerateKeys(string path="")
        {
            using (var rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        path = PATH;                                            
                    }
                    //公钥
                    var publicKey = rsa.ToXmlString(false);

                    //包括公钥私钥
                    var privateKey = rsa.ToXmlString(true);

                    // 保存到磁盘
                    File.WriteAllText(Path.Combine(path, PUBLIC_KEY_FILE_NAME), publicKey);
                    File.WriteAllText(Path.Combine(path, PRIVATE_KEY_FILE_NAME), privateKey);

                    Logger.Info(string.Format("生成的RSA密钥的路径: {0}\\ [{1}, {2}]", path, PUBLIC_KEY_FILE_NAME, PRIVATE_KEY_FILE_NAME));
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }


        /// <summary>
        /// 用给定路径的RSA公钥文件加密纯文本。
        /// </summary>
        /// <param name="plainText">要加密的文本</param>
        /// <param name="pathToPublicKey">用于加密的公钥路径.</param>
        /// <returns>表示加密数据的64位编码字符串.</returns>
        public string Encrypt(string plainText, string pathToPublicKey)
        {
            using (var rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
            {
                try
                {
                    //加载公钥
                    var publicXmlKey = File.ReadAllText(pathToPublicKey);

                    rsa.FromXmlString(publicXmlKey);

                    var bytesToEncrypt = System.Text.Encoding.Unicode.GetBytes(plainText);

                    var bytesEncrypted = rsa.Encrypt(bytesToEncrypt, false);

                    return Convert.ToBase64String(bytesEncrypted);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        /// <summary>
        /// Decrypts encrypted text given a RSA private key file path.给定路径的RSA私钥文件解密 加密文本
        /// </summary>
        /// <param name="encryptedText">加密的密文</param>
        /// <param name="pathToPrivateKey">用于加密的私钥路径.</param>
        /// <returns>未加密数据的字符串</returns>
        public string Decrypt(string encryptedText, string pathToPrivateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(RSA_KEY_SIZE))
            {
                try
                {
                    var privateXmlKey = File.ReadAllText(pathToPrivateKey);
                    rsa.FromXmlString(privateXmlKey);

                    var bytesEncrypted = Convert.FromBase64String(encryptedText);

                    var bytesPlainText = rsa.Decrypt(bytesEncrypted, false);

                    return System.Text.Encoding.Unicode.GetString(bytesPlainText);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}
