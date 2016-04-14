using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.Providers
{
    public class RegexProvider
    {
        public static string GetMatchValue(string input, string pattern)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
            {
                return Regex.Match(input, pattern, RegexOptions.IgnoreCase).Value;
            }
            return string.Empty;
        }

        public static string GetMatchValue(string input, string pattern, string resultPattern)
        {
            return GetMatchValue(input, pattern, resultPattern, RegexOptions.IgnoreCase);
        }

        public static string GetMatchValue(string input, string pattern, string resultPattern, RegexOptions options)
        {
            if (Regex.IsMatch(input, pattern, options))
            {
                return Regex.Match(input, pattern, options).Result(resultPattern);
            }
            return string.Empty;
        }

        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
    }

}
