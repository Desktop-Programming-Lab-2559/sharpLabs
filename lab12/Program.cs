// «Частотная обработка текста». Для выбранного текстового файла подсчитать сколько раз каждое слово встречается
// в данном файле. Реализуйте опцию поиска слова с выводом информации о нем. Найдите самое часто встречающиеся слово.
// Срок сдачи до 25 апреля. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace lab12
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine();
            if (args.Length < 1)
            {
                Console.WriteLine("Путь до файла не указан");
                Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
                return;
            }

            var filePath = args[0];
            if (!File.Exists(filePath))
            {
                var baseFolder = AppDomain.CurrentDomain.BaseDirectory;
                var tPath = string.Concat(baseFolder, args[0]);
                if (!File.Exists(tPath))
                {
                    Console.WriteLine("Указан не корректный путь");
                    return;
                }

                filePath = tPath;
            }
            
            var dict = new Dictionary<string, int>();
            foreach (var s in File.ReadLines(filePath))
            {
                var words = s.Split(' ');
                foreach (var checkedS in words.SelectMany(CheckWord).Where(x => !x.Equals(string.Empty)))
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

            var maxCount = dict.Values.Max();
            var mostCommonWord = dict.Where(pair => pair.Value == maxCount);
            Console.WriteLine($"Наибольшее число повторений {maxCount} ");
            foreach (var word in mostCommonWord)
            {
                Console.Write($"{word} ");
            }

            Console.WriteLine();
            Console.WriteLine("Enter number of most popular words you want to be shown");
            var number = Console.ReadLine();
            Console.WriteLine("Most popular");
            foreach (var p in dict.OrderBy(pair => pair.Value).Reverse()
                .Take(int.TryParse(number, out var num) ? num : 10))
            {
                Console.WriteLine(p);
            }

            
            // Console.WriteLine("All words");
            // foreach (var pair in dict)
            // {
            //     Console.WriteLine($"{pair.Key} : {pair.Value}");
            // }
        }

        public static string[] CheckWord(string word)
        {
            var regex = new Regex("[^a-zа-я]", RegexOptions.IgnoreCase);
            return regex.Split(word);
        }
    }
}