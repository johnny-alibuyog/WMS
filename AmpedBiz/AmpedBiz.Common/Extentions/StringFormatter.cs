using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Common.Extentions
{
    internal class StringFormatter
    {
        private static IEnumerable<string> _suffixes;
        private static IEnumerable<string> _specialWords;
        private static IEnumerable<string> _onesRomanNumerals;
        private static IEnumerable<string> _tensRomanNumerals;

        public StringFormatter()
        {
            _suffixes = new HashSet<string>();
            _specialWords = new HashSet<string>();
            _onesRomanNumerals = new HashSet<string>() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
            _tensRomanNumerals = new HashSet<string>() { "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC", "C" };
        }

        private bool IsAllUpperOrAllLower(string input)
        {
            return (input.ToLower().Equals(input) || input.ToUpper().Equals(input));
        }

        private string WordToProperCase(string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;

            // Standard case
            var value = CapitaliseFirstLetter(word);

            // Special cases:
            foreach (var suffix in _suffixes)
                value = ProperSuffix(value, suffix);

            // Special words:
            foreach (var specialWord in _specialWords)
                value = SpecialWords(value, specialWord);

            // Roman Numerals
            value = HandleRomanNumerals(value);

            return value;
        }

        private string ProperSuffix(string word, string prefix)
        {
            if (string.IsNullOrEmpty(word))
                return word;

            var lowerWord = word.ToLower();
            var lowerPrefix = prefix.ToLower();

            if (!lowerWord.Contains(lowerPrefix))
                return word;

            var index = lowerWord.IndexOf(lowerPrefix);

            // if the search string is at the end of the word ignore.
            if (index + prefix.Length == word.Length)
                return word;

            return word.Substring(0, index) + prefix + CapitaliseFirstLetter(word.Substring(index + prefix.Length));
        }

        private string SpecialWords(string word, string specialWord)
        {
            if (word.Equals(specialWord, StringComparison.InvariantCultureIgnoreCase))
                return specialWord;
            else
                return word;
        }

        private string HandleRomanNumerals(string word)
        {
            // assume nobody uses hundreds
            foreach (string number in _onesRomanNumerals)
            {
                if (word.Equals(number, StringComparison.InvariantCultureIgnoreCase))
                {
                    return number;
                }
            }

            foreach (string ten in _tensRomanNumerals)
            {
                foreach (string one in _onesRomanNumerals)
                {
                    if (word.Equals(ten + one, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ten + one;
                    }
                }
            }

            return word;
        }

        private string CapitaliseFirstLetter(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1).ToLower();
        }

        public virtual void SetSuffixes(IEnumerable<string> suffixes)
        {
            if (suffixes == null)
                return;

            _suffixes = suffixes;
        }

        public virtual void SetSpecialWords(IEnumerable<string> specialWords)
        {
            if (specialWords == null)
                return;

            _specialWords = specialWords;
        }

        public virtual string ToProperCase(string input)
        {
            if (input == null)
                return null;

            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var words = input.Split(' ')
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => WordToProperCase(word));

            return string.Join(" ", words);
        }
    }
}
