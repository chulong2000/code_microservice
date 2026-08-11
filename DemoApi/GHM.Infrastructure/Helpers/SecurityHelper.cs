using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GHM.Infrastructure.Helpers
{
    public sealed class SecurityHelper
    {
        public static string Key { get; set; }

        /// <summary>
        /// Encrypt string
        /// </summary>
        /// <param name="message"></param>
        /// <returns>string</returns>
        [ObsoleteAttribute("This method is obsolete. Call CallNewMethod instead.", true)]
        public static string EncryptString(string message)
        {
            byte[] results;
            UTF8Encoding utf8 = new();
            using MD5CryptoServiceProvider hashProvider = new();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(Key));


            using (TripleDESCryptoServiceProvider tdesAlgorithm = new())
            {
                tdesAlgorithm.Key = tdesKey;
                tdesAlgorithm.Mode = CipherMode.ECB;
                tdesAlgorithm.Padding = PaddingMode.PKCS7;
                byte[] dataToEncrypt = utf8.GetBytes(message);
                try
                {
                    ICryptoTransform encryptor = tdesAlgorithm.CreateEncryptor();
                    results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                }
                finally
                {

                    tdesAlgorithm.Clear();
                    hashProvider.Clear();
                }
            }
            return Convert.ToBase64String(results);

        }

        /// <summary>
        /// Decrypt string
        /// </summary>
        /// <param name="message"></param>
        /// <returns>string</returns>
        [ObsoleteAttribute("This method is obsolete. Call CallNewMethod instead.", true)]
        public static string DecryptString(string message)
        {
            byte[] results;
            UTF8Encoding utf8 = new();

            using MD5CryptoServiceProvider hashProvider = new();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(Key));

            using (TripleDESCryptoServiceProvider tdesAlgorithm = new())
            {
                tdesAlgorithm.Key = tdesKey;
                tdesAlgorithm.Mode = CipherMode.ECB;
                tdesAlgorithm.Padding = PaddingMode.PKCS7;
                byte[] dataToDecrypt = Convert.FromBase64String(message);
                try
                {
                    ICryptoTransform decryptor = tdesAlgorithm.CreateDecryptor();
                    results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
                }
                finally
                {

                    tdesAlgorithm.Clear();
                    hashProvider.Clear();
                }
            }
            return utf8.GetString(results);
        }
    }
}
