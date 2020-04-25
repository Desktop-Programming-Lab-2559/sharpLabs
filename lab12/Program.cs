// «Частотная обработка текста». Для выбранного текстового файла подсчитать сколько раз каждое слово встречается
// в данном файле. Реализуйте опцию поиска слова с выводом информации о нем. Найдите самое часто встречающиеся слово.
// Срок сдачи до 25 апреля. 

using System;
using System.IO;

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
                Console.WriteLine(baseFolder);
                var tPath = string.Concat(baseFolder, args[0]);
                if (!File.Exists(tPath))
                {
                    Console.WriteLine("Указан не корректный путь");
                    return;
                }

                filePath = tPath;
            }
            
            var dict = new Analyzer(filePath);
            
            if (dict.Count == 0)
            {
                Console.WriteLine("В файле нет слов");
                return;
            }

            foreach (var p in dict.Stat())
            {
                Console.WriteLine(p.ToString());
            }

            Console.WriteLine("Введите слово, о котором надо показать информацию");
            var oneWord = Console.ReadLine();
            Console.WriteLine(dict.WordInfo(oneWord));
        }
    }
}