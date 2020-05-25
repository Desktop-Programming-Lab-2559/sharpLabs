using System;

namespace lab15
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var hospital = new Hospital(2, 10, 10, 
                @"C:\Users\viktor\RiderProjects\Labs\lab15\log.log"))
            {
                try
                {
                    // var hospital = new Hospital(5, 5, 5);
                    hospital.StartSimulation();
                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            // Console.Write("#");
            // Console.ForegroundColor = ConsoleColor.Red;
            // Console.Write("#");
            // Console.ForegroundColor = ConsoleColor.Gray;
            // Console.Write("#");
        }
    }
}