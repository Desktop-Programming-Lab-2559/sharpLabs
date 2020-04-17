// 11.
// a) Реализовать класс комплексного числа и для него операции умножения, сложения, деления, нахождения модуля,
// возведения в степень и извлечения корня. Определите в классе событие, которое одним из параметров принимает
// объект класса производного от EventArgs. Генерируйте это событие при делении на 0.
// Примечание. Класс должен содержать не менее 3 различных конструкторов.
// b) Реализовать обобщенный класс вектор и все операции векторной алгебры, а также нахождение модуля и
// скалярного произведения двух векторов. Реализовать статическую функцию, которая проводит процесс ортогонализации
// переданного множества векторов. Аргументом этой функции является коллекция векторов.
// Реализовать способ сравнения векторов, методы преобразования в(из) массив(а) элементов соответствующего типа.
// При описании шаблона укажите ограничения: наличие пустого конструктора у типа-аргумента.
// Продемонстрируйте работу вашего обобщённого класса с классом комплексных чисел из пункта a) Срок сдачи до 11 апреля.

using System;

namespace lab11
{
    class Program
    {
        static void Main(string[] args)
        {
            // var c = new ComplexL(1,1);
            // c.DivideByZeroEvent += PrintEvent;
            // c.DivideByZeroEvent += PrintEventWithArgs;

            // var c2 = new ComplexL(0, 0);
            // Console.WriteLine(c2.Arg);
            // Console.WriteLine(c / c2);
            
            // int[] a = {1, 2, 3};
            // var b = new[] {1, 2, 3};
            // var v = new VectorL<int>(new int[]{1234, 2});

            if (true)
            {
                // var doubleArr_1 = new double[4] { 3.0, 2.0, 1.0, 1.0 };
                // var doubleArr_2 = new double[4] { 3.0, 3.0, 1.0, 2.0 };
                // var doubleArr_3 = new double[4] { 1.0, 2.0, 1.0, 2.0 };
                var basis = new[]
                {
                    new VectorL<double>(3.0, 2.0, 1.0, 1.0),
                    new VectorL<double>(3.0, 3.0, 1.0, 2.0),
                    new VectorL<double>(1.0, 2.0, 1.0, 2.0)
                };
                foreach (var v in VectorL<double>.Orthogonality(basis))
                {
                    Console.WriteLine(v);
                }

                
            }

            if (false) {
                var basis = new[]
                {
                    new VectorL<ComplexL>(ComplexL.One, ComplexL.Zero, ComplexL.Zero),
                    new VectorL<ComplexL>(ComplexL.Zero, ComplexL.One, ComplexL.Zero),
                    new VectorL<ComplexL>(ComplexL.Zero, ComplexL.Zero, ComplexL.One),
                };

                var newBasis = VectorL<ComplexL>.Orthogonality(basis);
                newBasis.ForEach(Console.WriteLine);
                Console.WriteLine($"Vec length = {newBasis[0].Abs()}");
            }

            if (false)
            {
                VectorL<int> a = new VectorL<int>(3, 2, 2), b = new VectorL<int>(3, 2, 2);
                Console.WriteLine(a == b);
            }

            if (false)
            {
                var v = new VectorL<int>();
                Console.WriteLine(v);
            }
        }

        private static void PrintEvent(object sender, ComplexEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
        
        private static void PrintEventWithArgs(object sender, ComplexEventArgs args)
        {
            Console.WriteLine($"{args.Message} first = {args.First} second = {args.Second} is zero");
        }
    }
}