// Срок сдачи до 21 марта. Дана строка.
// a) Подсчитать сколько раз в этой строке встречается заданное слово.
// b) Заменить в данной строке предпоследнее слово на слово, которое ввел пользователь.
// c) Найти 𝑘 − ое слово в строке начинающиеся с заглавной буквы. 

using System;
using System.Linq;
using Microsoft.VisualBasic;

namespace lab08
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var mainString = args[0];

            // a
            Console.WriteLine("Enter substring");
            var word = Console.ReadLine();
            if (word == null) return;
            var copy = (string) mainString.Clone();
            var count = (mainString.Length - copy.Replace(word, "").Length) / word.Length;
            // count = mainString.Count(word);
            Console.WriteLine($"Count = {count}");

            // b
            var arr = mainString.Split(" ");
            if (arr.Length >= 2) arr[^2] = word;
            Console.WriteLine(Strings.Join(arr));

            // c
            var sNum = Console.ReadLine();
            if (sNum == null) return;
            if (int.TryParse(sNum, out var num))
            {
                arr = mainString.Split(" ");
                arr = arr.Where(x => char.IsUpper(x[0])).ToArray();
                try
                {
                    Console.WriteLine($"\"{arr[num - 1]}\"");
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("Array contain less elements than entered num");
                    if (arr.Length != 0) Console.WriteLine($"Last word \"{arr[^1]}\"");
                }
            }
        }
    }
}