using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Utility.Security
{
    public class AesCryptoEngine : IAesCryptoEngine
    {
        private readonly byte[] Key;
        private readonly byte[] IV;
        public AesCryptoEngine(IConfiguration configuration)
        {
             this.Key = Convert.FromBase64String(configuration["AES:Key"]);
             this.IV = Convert.FromBase64String(configuration["aes:IV"]);
        }

        public string Encrypt(string dataToEncrypt)
        {
            var x = Encrypt(Encoding.UTF8.GetBytes(dataToEncrypt));
            return Convert.ToBase64String(x);
        }

        public string DeCrypt(string dataToDecrypt)
        {
            var x = Decrypt(Encoding.UTF8.GetBytes(dataToDecrypt));
            return Convert.ToBase64String(x);
        }
        private  byte[] Encrypt(byte[] dataToEncrypt)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = Key;
                aes.IV = IV;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        private  byte[] Decrypt(byte[] dataToDecrypt)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = Key;
                aes.IV = IV;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    var decryptBytes = memoryStream.ToArray();

                    return decryptBytes;
                }
            }
        }
    }
}
