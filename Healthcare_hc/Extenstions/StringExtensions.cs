
using System.Text;
using System.Text.RegularExpressions;

namespace HealthCare_Common.Extensions
{
    public static class StringExtensions
    {
       
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static string StripLeadingWhitespace(this string str)
        {
            var r = new Regex(@"^\s+", RegexOptions.Multiline);
            return r.Replace(str, string.Empty);
        }

        public static string ToTitleCase(this string str)
        {
            var value = str.Trim();

            var sb = new StringBuilder();

            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] == ' ')
                {
                    sb.Append(' ');
                    continue;
                }

                if (i == 0 || value[i - 1] == ' ')
                {
                    sb.Append(char.ToUpper(value[i]));
                    continue;
                }

                sb.Append(char.ToLower(value[i]));
            }

            return sb.ToString();
        }

        public static string ToUrlizedCase(this string str)
        {
            var value = str.Trim().ToLower().Replace(" ", "_");

            return value;
        }
    }
}