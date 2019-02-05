using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CakeExtracter.Common
{
    public static class TextUtils
    {
        public static string RemoveFirstLine(string text)
        {
            var lines = Regex.Split(text, "\r\n|\r|\n").Skip(1);
            return string.Join(Environment.NewLine, lines.ToArray());
        }
    }
}
