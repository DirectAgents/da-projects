using System.Text;

namespace DAgents.Common.AsciiArt
{
    public static class AsciiArtExtensions
    {
        public static string InBox(this string s)
        {
            int len = s.Length;
            var sb = new StringBuilder();

            sb.Append("+");
            sb.Append('-', len);
            sb.Append("+");
            sb.AppendLine();

            sb.Append("|");
            sb.Append(s);
            sb.Append("|");
            sb.AppendLine();

            sb.Append("+");
            sb.Append('-', len);
            sb.Append("+");
            sb.AppendLine();

            return sb.ToString();
        }

        public static string AfterArrow(this string s)
        {
            int len = s.Length;
            var sb = new StringBuilder();

            sb.AppendLine("|");
            sb.Append("+-->> ");
            sb.Append(s);
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
