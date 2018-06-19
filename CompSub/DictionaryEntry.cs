using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CompSub
{
    public class DictionaryEntry
    {
        public Regex Regex { get; }
        public string Value { get; }

        public DictionaryEntry(string regex, string value)
        {
            Regex = new Regex(regex, RegexOptions.IgnoreCase);
            Value = value;
        }

        public static List<DictionaryEntry> GetAll()
        {
            List<DictionaryEntry> dictionary = new List<DictionaryEntry>();

            dictionary.Add(new DictionaryEntry("first.*name|initials|fname|first$|given.*name", "Karl"));
            dictionary.Add(new DictionaryEntry("last.*name|lname|surname|last$|secondname|family.*name", "Hornlund"));
            dictionary.Add(new DictionaryEntry("(\"^name|full.?name|your.?name|customer.?name|bill.?name|ship.?name\")", "Karl Hornlund"));
            dictionary.Add(new DictionaryEntry("e.?mail", "karlhornlund.ozb@gmail.com"));

            return dictionary;
        }
    }
}
