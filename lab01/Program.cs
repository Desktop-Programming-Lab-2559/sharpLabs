//Напишите две функции для поиска всех корней кубического уравнения двумя разными способами: с помощью формулы Кордано
//    и без нее. Результат возвращайте через заголовок функции. Сравните полученные результаты. Срок сдачи до 14 марта. 
using System;
using System.Numerics;

namespace Lab01
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Enter coefficient of a*x^3+b*x^2+c*x+d=0");
            var coef = new double[4];
            for (var i = 0; i < 4; i++)
            {
                Console.Write($"{Convert.ToChar(97 + i)} = ");
                var s = Console.ReadLine();

                if (double.TryParse(s, out var c))
                {
                    coef[i] = c;
                }
                else
                {
                    Console.WriteLine($"Cannot parse coefficient \"{s}\"");
                    return;
                }
            }

            Console.Write($"Input equation {coef[0]}x^3");
            Console.Write(coef[1] < 0 ? $"{coef[1]}x^2" : $"+{coef[1]}x^2");
            Console.Write(coef[2] < 0 ? $"{coef[2]}x" : $"+{coef[2]}x");
            Console.WriteLine(coef[3] < 0 ? $"{coef[3]}=0" : $"+{coef[3]}=0");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Kordano solve");
            Console.ForegroundColor = ConsoleColor.Gray;
            SolveKordano(coef, out var first, out var second, out var third);
            PrintSolve(first, "First");
            PrintSolve(second, "Second");
            PrintSolve(third, "Third");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Vieta solve");
            Console.ForegroundColor = ConsoleColor.Gray;
            SolveVieta(coef, out first, out second, out third);
            PrintSolve(first, "First");
            PrintSolve(second, "Second");
            PrintSolve(third, "Third");
        }

        private static void SolveKordano(double[] coefficients, out Complex fSolve, out Complex sSolve,
            out Complex tSolve)
        {
            double a = coefficients[0], b = coefficients[1], c = coefficients[2], d = coefficients[3];

            var p = (3 * a * c - b * b) / (3 * a * a);
            var q = (2 * Math.Pow(b, 3) - 9 * a * b * c + 27 * a * a * d) / (27 * Math.Pow(a, 3));

            var Q = Math.Pow(p / 3, 3) + Math.Pow(q / 2, 2);

            if (Q < 0)
            {
                var sqrtQ = Complex.Pow(Q, 1.0 / 2.0);

                var alpha = Complex.Pow(-q / 2 + sqrtQ, 1.0 / 3.0);
                var beta = Complex.Pow(-q / 2 - sqrtQ, 1.0 / 3.0);

                fSolve = alpha + beta - b / (3 * a);
                sSolve = -(alpha + beta) / 2 + Complex.ImaginaryOne * (alpha - beta) / 2 * Math.Sqrt(3) - b / (3 * a);
                tSolve = -(alpha + beta) / 2 - Complex.ImaginaryOne * (alpha - beta) / 2 * Math.Sqrt(3) - b / (3 * a);
            }
            else if (Q > 0)
            {
                var sqrtQ = Math.Sqrt(Q);

                var tmp = -q / 2 + sqrtQ;
                var alpha = Math.Sign(tmp) * Math.Pow(Math.Abs(tmp), 1.0 / 3);
                tmp = -q / 2 - sqrtQ;
                var beta = Math.Sign(tmp) * Math.Pow(Math.Abs(tmp), 1.0 / 3);

                fSolve = alpha + beta - b / (3 * a);
                sSolve = -(alpha + beta) / 2 + Complex.ImaginaryOne * (alpha - beta) / 2 * Math.Sqrt(3) - b / (3 * a);
                tSolve = -(alpha + beta) / 2 - Complex.ImaginaryOne * (alpha - beta) / 2 * Math.Sqrt(3) - b / (3 * a);
            }
            else
            {
                fSolve = 2 * Math.Pow(-q / 2, 1.0 / 3) - b / (3 * a);
                sSolve = tSolve = -Math.Pow(-q / 2, 1.0 / 3) - b / (3 * a);
            }
        }

        private static void PrintSolve(Complex c, string name)
        {
            Console.Write($"{name} solve = ");
            Console.Write(Math.Abs(c.Real) <= double.Epsilon ? 0 : c.Real);
            if (Math.Abs(c.Imaginary) >= double.Epsilon)
                Console.Write(c.Imaginary > 0 ? $"+{c.Imaginary}i" : $"{c.Imaginary}i");
            Console.WriteLine();
        }

        private static void SolveVieta(double[] coefficients, out Complex fSolve, out Complex sSolve,
            out Complex tSolve)
        {
            var tmp = new double[4];
            Array.Copy(coefficients, tmp, 4);
            for (var i = 0; i < coefficients.Length; ++i) tmp[i] /= coefficients[0];

            double a = tmp[1], b = tmp[2], c = tmp[3];

            double Q = (a * a - 3 * b) / 9, R = (2 * a * a * a - 9 * a * b + 27 * c) / 54;
            var S = Q * Q * Q - R * R;

            if (S > 0)
            {
                var phi = Math.Acos(R / Math.Pow(Q, 3.0 / 2)) / 3;

                fSolve = -2 * Math.Sqrt(Q) * Math.Cos(phi) - a / 3;
                sSolve = -Math.Sqrt(Q) * Math.Cos(phi + 2 * Math.PI / 3) - a / 3;
                tSolve = -Math.Sqrt(Q) * Math.Cos(phi - 2 * Math.PI / 3) - a / 3;
            }
            else if (S < 0)
            {
                if (Q > 0)
                {
                    var phi = Math.Acosh(Math.Abs(R) / Math.Pow(Math.Abs(Q), 3.0 / 2)) / 3;

                    fSolve = -2 * Math.Sign(R) * Math.Sqrt(Math.Abs(Q)) * Math.Cosh(phi) - a / 3;
                    sSolve = Math.Sign(R) * Math.Sqrt(Math.Abs(Q)) * Math.Cosh(phi) - a / 3 +
                             Math.Sqrt(3 * Math.Abs(Q)) * Math.Sinh(phi) * Complex.ImaginaryOne;
                    tSolve = Math.Sign(R) * Math.Sqrt(Math.Abs(Q)) * Math.Cosh(phi) - a / 3 -
                             Math.Sqrt(3 * Math.Abs(Q)) * Math.Sinh(phi) * Complex.ImaginaryOne;
                }
                else if (Q < 0)
                {
                    var phi = Math.Asinh(Math.Abs(R) / Math.Pow(Math.Abs(Q), 3.0 / 2)) / 3;

                    fSolve = -2 * Math.Sign(R) * Math.Sqrt(Math.Abs(Q)) * Math.Sinh(phi) - a / 3;

                    double real = Math.Sign(R) * Math.Sqrt(Math.Abs(Q)) * Math.Sinh(phi) - a / 3,
                        im = Math.Sqrt(3 * Math.Abs(Q)) * Math.Cosh(phi);
                    sSolve = real + Complex.ImaginaryOne * im;
                    tSolve = real - Complex.ImaginaryOne * im;
                }
                else
                {
                    fSolve = -Math.Cbrt(c - a * a * a / 27) - a / 3;
                    var solve = -Math.Cbrt(c - a * a * a / 27) - a / 3;
                    double real = -(a + solve) / 2,
                        im = 0.5 * Math.Sqrt(Math.Abs((a - 3 * solve) * (a + solve) - 4 * b));
                    sSolve = real + Complex.ImaginaryOne * im;
                    tSolve = real - Complex.ImaginaryOne * im;
                }
            }
            else
            {
                fSolve = -2 * Math.Pow(R, 1.0 / 3) - a / 3;
                sSolve = tSolve = Math.Pow(R, 1.0 / 3) - a / 3;
            }
        }
    }
}