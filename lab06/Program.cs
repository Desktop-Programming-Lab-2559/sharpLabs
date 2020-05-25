// Прочитать с консоли произвольное количество слов, отсортировать их по алфавиту и вывести на экран слово,
// состоящее из последних символов этих слов. Срок сдачи до 21 марта. 

using System;
using System.Collections.Generic;
using System.Linq;

namespace lab06
{
    class Program
    {
        private static void Main(string[] args)
        {
            var list = new List<string>();
            string s;
            while ((s = Console.ReadLine()) != "") list.Add(s);

            list.Sort();
            foreach (var f in list) Console.Write(f.Last());

            Console.WriteLine();
        }
    }
}