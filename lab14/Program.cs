// «Построитель СДНФ/СКНФ». С использованием стандартных коллекций реализовать программу, которая для любой
// булевой формулы строит СДНФ и СКНФ. Программа должна не только выводить ответ, но и выводить трассировку своей
// работы. При этом входными данными программы является текстовый файл, в котором находится произвольное число
// булевых формул. Результатом работы программы является файл с СДНФ и СКНФ для каждой формулы. Срок сдачи до 30 мая.

using System;
using System.Linq;
using System.Text;

namespace lab14
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Operation.Tracing += Console.WriteLine;
            // var op1 = new Operation("!(a|(a|b)&!c)~d");
            // var op2 = new Operation("!((A|B)&!C)~D");
            // var op3 = new Operation("!(!(!(A|B)&C)+C)");
            // var op4 = new Operation("!!!(A|B)&C+C");
            // var op5 = new Operation("a|b&c");
            // Console.WriteLine(op);
            // Console.WriteLine(op.OperationString);
            //
            // var opCollection = new Operations ();
            // opCollection.Add(op);
            // opCollection.Add(oOp);
            // opCollection.Save("C:/Users/viktor/RiderProjects/Labs/lab14/test.txt");
            
            var loadedCollection = Operations.Load("C:/Users/viktor/RiderProjects/Labs/lab14/test.json");
            foreach (Operation operation in loadedCollection)
            {
                Console.WriteLine(operation);
                Console.WriteLine($"PCNF {operation.PCNF()}");
                Console.WriteLine($"PDNF {operation.PDNF()}");
                Console.WriteLine();
            }

            // var table = op.CalculateTable();
            // foreach (var bools in table)
            // {
            //     foreach (var b in bools)
            //     {
            //         Console.Write(b ? "1 " : "0 ");
            //     }
            //
            //     Console.WriteLine();
            // }
        }

        public static void Tracing(string text) => Console.WriteLine(text);
    }
}