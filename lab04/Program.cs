// Считать как параметры командной строки 2 числа: 1-число в десятичной системе счисления, 2- новая система счисления.
// Перевести число 1 в систему счисления 2 и вывести на консоль. Срок сдачи до 14 марта
using System;
using System.Text;

namespace lab04
{
    class Program
    {
        static void Main(string[] args)
        {
            args[0] = "123456";
            args[1] = "16";
            if (args.Length < 2)
            {
                Console.WriteLine("Wrong number of arguments");
                return;
            }

            if (!int.TryParse(args[0], out int num))
            {
                Console.WriteLine("Cannot parse first argument");
                return;
            }
            
            if (!int.TryParse(args[1], out int baseNum))
            {
                Console.WriteLine("Cannot parse second argument");
                return;
            }

            StringBuilder res = new StringBuilder();
            while (num != 0)
            {
                Math.DivRem(num, baseNum, out int rem);
                var c = rem > 9 ? Convert.ToChar('A' + rem - 10).ToString() : rem.ToString();
                res.Insert(0, c);
                num /= baseNum;
            }

            Console.WriteLine($"{args[0]} = {res}({args[1]})");
        }
    }
}