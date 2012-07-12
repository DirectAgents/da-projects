using System.Text.RegularExpressions;

namespace EomApp1.Screens.Synch
{
    public static class NameHelper
    {
        public static string NormalizeName(string name)
        {
            string result = string.Empty;
            foreach (Match match in Regex.Matches(name, @"\w*"))
            {
                string s = match.Captures[0].Value;
                if (!string.IsNullOrEmpty(s))
                {
                    result += s.ToUpper();
                }
            }
            return result;
        }
    }
}
