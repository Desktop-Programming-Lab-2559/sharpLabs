// Напишите функцию, которая находит корень уравнения методом дихотомии.
// Аргументами функции являются границы интервала, на котором находится корень,
// делегат, связанный с уравнением, и точность, с которой корень необходимо найти.
// Срок сдачи до 28 марта.
using System;

namespace lab09
{
    delegate double Equation(double x);
    
    class Program
    {
        static void Main(string[] args)
        {
            // x => x * x * x - 4 * x * x + x + 2
            Console.WriteLine(FindRoot(1, 3, 
                x => x*x - 4, 1e-100));
        }

        static double FindRoot(double left, double right, Equation equation, double precision)
        {
            if (right < left)
            {
                double t = left;
                left = right;
                right = t;
            }
            
            if (equation(left) * equation(right) > 0)
            {
                Console.WriteLine("The equation have same sign on ends of the segment.\n" +
                                  "It is possible that program may not to able to find the root");
            }
            
            double root = (left + right) / 2;
            while (Math.Abs(equation(root)) > precision)
            {
                root = (left + right) / 2;
                if (equation(left) * equation(root) < 0)
                {
                    right = root;
                }
                else
                {
                    left = root;
                }

                if (Math.Abs(left - right) < double.Epsilon) return double.NaN;
            }

            return root;
        }
    }
}