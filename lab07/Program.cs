// Дана строка. Получить новую строку, в которой все символы исходной строки отсортированы по возрастанию.
// Срок сдачи до 21 марта.

using System;

namespace lab07
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Enter string");
            var s = Console.ReadLine()?.ToCharArray();
            if (s == null || s.Length == 0)
                return;
            Array.Sort(s);
            Console.WriteLine(string.Join(string.Empty, s));
        }
    }
}