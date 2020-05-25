// a) Прочитать с консоли произвольное число символов, разделенных пробелом, и вывести их среднее арифметическое.
//     Примечание. Ввод символа «не цифры» является корректным. Срок сдачи до 14 марта.
// b) Прочитать с консоли произвольное число чисел и найти их среднее геометрическое. При вводе недопустимого символа
//     вывести информацию об ошибке. Примечание. Допускается ввод целых и вещественных чисел. Разделителем целой и
//     дробной части является символ точки или запятой. Срок сдачи до 14 марта.

using System;
using System.Collections.Generic;
using System.Linq;

namespace lab03
{
    class Program
    {
        private static void Main(string[] args)
        {
            PartA();
            PartB();
        }

        private static void PartA()
        {
            Console.WriteLine("Part A\nEnter something");
            var s = Console.ReadLine();
            if (s == null) return;
            var sum = s.Aggregate(0, (current, some) => current + some);

            Console.WriteLine();
            Console.WriteLine($"Avg = {(double) sum / s.Length}");
        }

        private static void PartB()
        {
            Console.WriteLine("Part B");
            var list = new List<double>();
            var s = Console.ReadLine();
            while (s != "")
            {
                s = s.Replace(".", ",");
                if (double.TryParse(s, out var c))
                    list.Add(c);
                else
                    Console.WriteLine("Uncorrected data");
                s = Console.ReadLine();
            }

            double value = 1;
            Console.Write("Used ");
            foreach (var v in list)
            {
                value *= v;
                Console.Write($"{v} ");
            }

            Console.WriteLine();
            Console.WriteLine(
                $"Gmean = {(list.Count != 0 ? Math.Sign(value) * Math.Pow(Math.Abs(value), 1.0 / list.Count) : 0)}");
        }
    }
}