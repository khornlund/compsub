using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CompSub
{
    public class DictionaryEntry
    {
        public Regex Regex { get; }
        public string Value { get; }

        private static List<DictionaryEntry> _dictionary = null;

        public DictionaryEntry(string regex, string value)
        {
            Regex = new Regex(regex, RegexOptions.IgnoreCase);
            Value = value;
        }

        private static IEnumerable<string> ReadConfig()
        {
            string filename = @"../../../config.txt";

            using (StreamReader sr = new StreamReader(filename))
            {
                while (sr.Peek() >= 0 )
                {
                    yield return sr.ReadLine();
                }
            }
        }

        private static void InitialiseDictionary()
        {
            _dictionary = new List<DictionaryEntry>();

            foreach (var line in ReadConfig())
            {
                Logger.Log("Adding: " + line);
                string[] split = line.Split(',');
                var entry = new DictionaryEntry(split[0], split[1]);
                _dictionary.Add(entry);
                Logger.Log($"Created regex={entry.Regex}, value={entry.Value}");
            }
        }

        public static List<DictionaryEntry> GetAll()
        {
            if (_dictionary == null)
                InitialiseDictionary();

            return _dictionary;
        }
    }
}
