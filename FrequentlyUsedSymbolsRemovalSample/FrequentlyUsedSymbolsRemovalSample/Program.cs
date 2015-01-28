using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FrequentlyUsedSymbolsRemovalSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("text.txt");
            var textFilter = new TextFilter(text);
            var message = textFilter.CleanupText();

            Console.WriteLine(message);
            Console.Read();
        }
    }

    public class TextFilter
    {
        private readonly string _text;
        public TextFilter(string input)
        {
            this._text = input;
        }

        public string CleanupText()
        {
            var chars = GetRareChars();
            var message = String.Join("", _text.Where(chars.Contains));
            return message;
        }

        private HashSet<char> GetRareChars()
        {
            var chars = GetCharsWithFrequencyOfUsage()
                    .OrderByDescending(o => o.Value).ToList();
            var index = FindIndexOfSplitCommonAndRareChars(chars);
            var rareChars = chars.Skip(index).Select(o => o.Key);
            return new HashSet<char>(rareChars);
        }

        private Dictionary<char, int> GetCharsWithFrequencyOfUsage()
        {
            var chars = _text.GroupBy(ch => ch)
                .ToDictionary(group => group.Key, group => group.Count());
            return chars;
        }

        private int FindIndexOfSplitCommonAndRareChars(IReadOnlyList<KeyValuePair<char, int>> chars)
        {
            var maxChangeInUsage = 0.0;
            var terminator = 0;

            for (var i = 0; i < chars.Count - 1; i++)
            {
                var changeInUsage = 1.0 * chars[i].Value / chars[i + 1].Value;
                if (changeInUsage > maxChangeInUsage)
                {
                    maxChangeInUsage = changeInUsage;
                    terminator = i;
                }
            }
            return terminator + 1;
        }
    }
}
