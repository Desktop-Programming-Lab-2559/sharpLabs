using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace lab12
{
    public class Analyzer
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();

        public Analyzer(string path)
        {
            foreach (var s in File.ReadLines(path))
            {
                var words = s.Split(' ');
                foreach (var checkedS in words.SelectMany(CheckWord))
                {
                    var edited = checkedS.ToLower();
                    if (dict.ContainsKey(edited))
                    {
                        dict[edited] += 1;
                    }
                    else
                    {
                        dict[edited] = 1;
                    }
                }
            }
        }

        public static string[] CheckWord(string word)
        {
            var regex = new Regex("[^a-zа-я]", RegexOptions.IgnoreCase);
            return regex.Split(word).Where(x => !x.Equals(string.Empty)).ToArray();
        }

        public int Count => dict.Count;
        public int AllWordsCount => dict.Values.Sum();
        // public int MostCommon => dict.Values.Max();
        public IEnumerable MostPopularWord()
        {
            var maxCount = dict.Values.Max();
            return dict.Where(pair => pair.Value == maxCount);
        }

        public IEnumerable MostPopular(int size)
        {
            return dict.OrderBy(pair => pair.Value).Reverse().Take(dict.Count > size ? size : dict.Count);
        }

        public int WordInfo(string word)
        {
            return dict.ContainsKey(word) ? dict[word] : 0;
        }

        public Dictionary<string, int> Stat()
        {
            return new Dictionary<string, int>(dict);
        }
    }
}