using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NewsTicker
{
    public static class Common
    {
        public static byte[] GetHash(string val)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(val));
        }

        public static string HashValue(string val)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(val))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string Hash(this string val)
        {
            return HashValue(val);
        }
    }
}
