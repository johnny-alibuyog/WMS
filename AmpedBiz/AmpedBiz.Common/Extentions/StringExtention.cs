using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AmpedBiz.Common.Extentions
{
    public static class StringExtention
    {
        private static readonly StringFormatter _formatter = new StringFormatter();

        public static void SetProperCaseSuffix(IEnumerable<string> suffixes)
        {
            _formatter.SetSuffixes(suffixes);
        }

        public static void SetProperCaseSpecialWords(IEnumerable<string> specialWords)
        {
            _formatter.SetSpecialWords(specialWords);
        }

        public static string ToProperCase(this string input)
        {
            return _formatter.ToProperCase(input);
        }

        public static bool IsEqualTo(this string stringA, string stringB)
        {
            var fixedStringA = Regex.Replace(stringA ?? string.Empty, @"\s+", string.Empty);
            var fixedStringB = Regex.Replace(stringB ?? string.Empty, @"\s+", string.Empty);

            return String.Equals(fixedStringA, fixedStringB, StringComparison.OrdinalIgnoreCase);
        }

		public static bool IsNullOrWhiteSpace(this string value)
		{
			return string.IsNullOrWhiteSpace(value);
		}

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}
