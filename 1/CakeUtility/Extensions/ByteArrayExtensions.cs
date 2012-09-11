using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectAgents.Common
{
    public static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] bytes)
        {
            var result = "";

            foreach (byte b in bytes)
            {
                result += b.ToString("X2").ToLower();
            }

            return (result);
        }
    }
}
