using System.Security.Cryptography;
using System.Text;
using System;

namespace DirectAgents.Common
{
    public static class StringExtensions
    {
        public static byte[] ComputeHash(this string target, string key, Func<byte[], HashAlgorithm> algorithm)
        {
            var encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(target);

            byte[] hashMessage;
            var hash = algorithm(keyBytes);
            hashMessage = hash.ComputeHash(messageBytes);

            return hashMessage;
        }
    }
}
