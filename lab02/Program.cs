// Напишите функцию, вычисляющую числа 𝑒,𝜋 иln2 с точностью 15 знаков после запятой. Срок сдачи до 14 марта.
using System;

namespace lab02
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"e  = {E()}");
            Console.WriteLine("e  = 2,7182818284590452");
            Console.WriteLine();
            Console.WriteLine($"pi = {Pi()}");
            Console.WriteLine("pi = 3,1415926535897932");
            Console.WriteLine();
            Console.WriteLine($"ln = {Ln2()}");
            Console.WriteLine("ln = 0,6931471805599453");
        }

        private static double E()
        {
            double epsilon = 1e-15, e = 0, c = 1;
            int i = 1;
            while (1/c > epsilon)
            {
                e += 1/c;
                c *= i;
                ++i;
            }

            return e;
        }

        private static double Pi()
        {
            //Ряд Нилаканта
            double pi = 3, epsilon = 1e-5;
            int i = 3, sign = 1;
            double c;
            while (Math.Abs(c = 4.0 / ((i - 1) * i * (i + 1))) > epsilon)
            {
                pi += sign * c;
                i += 2;
                sign *= -1;
            }

            return pi;
        }

        private static double Ln2()
        {
            double epsilon = 1e-15, ln = 0;
            double c = 0.5;
            int i = 2;
            while (c > epsilon)
            {
                ln += c;
                c = 1 / Math.Pow(2, i) / i++;
            }
            return ln;
        }
    }
}