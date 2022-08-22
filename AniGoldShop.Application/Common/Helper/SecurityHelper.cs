using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AniGoldShop.Application.Common.Helper
{
    public sealed class SecurityHelper 
    {
        private static byte[] ivKey = new byte[] { 27, 102, 35, 35, 1, 8, 98, 25, 128, 147, 81, 65, 32, 78, 127, 253 };
        /// <summary>
        /// Generice confirm code
        /// </summary>
        /// <returns>6 character digit confirm code</returns>
        public static string GenerateSmsConfirmCode()
        {      
            var minute = DateTime.UtcNow.Minute;
            var second = DateTime.UtcNow.Second;

            var testCode = (minute + second) % 60;

            if (testCode > 10)
                testCode = 0;
            else
                testCode = 60 - testCode;

            var paddingChar = '0';
            var confirmCode = string.Format("{0}{1}{2}",
                minute.ToString().PadLeft(2, paddingChar),
                second.ToString().PadLeft(2, paddingChar),
                testCode);

            return confirmCode;
        }

        public static string GenerateEmailConfirmCode()
        {
            Guid g = new Guid();
            g = Guid.NewGuid();

            var confirmCode = g.ToString();

            return confirmCode;
        }

        public static string GetHash(string value)
        {
            using (var sha = SHA256.Create())
            {
                var hashbyte = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
                var hash = BitConverter.ToString(hashbyte).Replace("-", "").ToLower();
                return hash;
            }
        }

        public static string AESDecrypt(string securityKey, string encryptedValue)
        {
            var key = Encoding.UTF8.GetBytes(securityKey);
            var bufferEncryptedValue = Convert.FromBase64String(encryptedValue);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = ivKey;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream mStream = new MemoryStream(bufferEncryptedValue);
                using CryptoStream cryptoStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new StreamReader(cryptoStream);
                return streamReader.ReadToEnd();
            }
        }

        public static string AESEncrypt(string securityKey,string value)
        {
            var key = Encoding.UTF8.GetBytes(securityKey);

            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = ivKey;

                ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(
                    msEncrypt, 
                    cryptoTransform, 
                    CryptoStreamMode.Write
                ))
                {
                    using StreamWriter swEncrypt = new StreamWriter(csEncrypt);
                    swEncrypt.Write(value);
                }
                encrypted = msEncrypt.ToArray();
            }

            return Convert.ToBase64String(encrypted);
        }

        public static string GenerateCode()
        {
            var year = DateTime.UtcNow.Year;
            var month = DateTime.UtcNow.Month;
            var day = DateTime.UtcNow.Day;

            var days = year * 365 + month * 30 + day;

            var hour = DateTime.UtcNow.Hour;
            var minute = DateTime.UtcNow.Minute;
            var second = DateTime.UtcNow.Second;

            var seconds = hour * 60 * 60 + minute * 60 + second;

            return (days + seconds).ToString();
        }
    }
}
