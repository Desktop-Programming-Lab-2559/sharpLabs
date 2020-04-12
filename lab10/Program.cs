//  a) Реализовать класс квадратной матрицы произвольного размера и для него операции сложения, умножения,
//     деления, нахождение определителя и транспонирования. При невозможности выполнить какую-либо операцию
//     генерируйте исключение (Объект исключение должен быть объектом пользовательского класса,
//     пронаследованного от Exception). Реализовать интерфейс ICloneable. Добавить возможность работы с элементами
//     матрицы в цикле foreach. Примечание. Класс должен содержать не менее 3 различных конструкторов.
// b) Реализуйте обобщенный класс полиномов. Ваш класс должен содержать не менее 3 конструкторов, иметь перегрузки
//     всех стандартных арифметических операций над полиномами, иметь метод вычисления значения полинома
//     в заданной точке и метод позволяющий вычислять композицию двух полиномов. Реализуйте возможность копирования
//     полиномов, сравнения полиномов и возможность выбирать коэффициенты в цикле foreach.
//     Для демонстрации возможностей вашего класса используйте класс матрицы из пункта а)  Срок сдачи до 11 апреля. 
using System;
using System.Runtime.InteropServices;

namespace lab10
{
    class Program
    {
        static void Main(string[] args)
        {
            // var m = new Matrix(1, 1, 0, 0);
            // Console.WriteLine(m.Determinant());
            // try
            // {
            //     Console.WriteLine(Matrix.Inverse(m));
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     throw;
            // }
            // m.TransposeThis();
            // Console.WriteLine(m.TransposeNew());
            
            // Polynomial<int> p1 = new Polynomial<int>(1, 1), p2 = new Polynomial<int>(1, 1);
            // Console.WriteLine(p1 * p2);
            // Polynomial<double> p = new Polynomial<double>(5, -3, 7, 2, -3, 1), 
            // q = new Polynomial<double>(1, -1, 1);
            
            // Polynomial<double> p = new Polynomial<double>(4, 2, 0, 5, 3), q = new Polynomial<double>(1, 3, 1);
            // Console.WriteLine(q + p);
            
            // Polynomial<Matrix> p = new Polynomial<Matrix>(new Matrix(1, 2, 3, 4)),
            // q = new Polynomial<Matrix>(new Matrix(1, 0, 0, 1));
            
            // Polynomial<int> p = new Polynomial<int>(-1, 0, 1), q = new Polynomial<int>(1, 0, 1);
            // Console.WriteLine(p - q);
            // Console.WriteLine(p * q);

            // foreach (var v in p)
            // {
            //     Console.WriteLine(v);
            // }

            // Polynomial<double> p = new Polynomial<double>(1, 1), q = new Polynomial<double>(-1, -2, 1);
            // Console.WriteLine(p.Compose(q));
            // Console.WriteLine(p * q);
        }
    }
}