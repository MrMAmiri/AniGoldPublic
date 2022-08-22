using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AniGoldShop.Infrastructure.Common.Extensions
{
    public static class SecurityExtension
    {
        public static string ToHSA256(this string value,string key)
        {
            var enc = Encoding.UTF8;
            var stringBuilder = new StringBuilder();

            using(var hash = new HMACSHA256(enc.GetBytes(key)))
            {
                var hashed = hash.ComputeHash(enc.GetBytes(value));

                foreach (var b in hashed)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
            }

            return stringBuilder.ToString();
        }
    }
}
