using System;

namespace lab15
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var hospital = new Hospital(1, 10, 10, @"C:\Users\viktor\RiderProjects\Labs\lab15\log.log");
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
            finally
            {
                hospital.Dispose();
            }

            // Console.Write("#");
            // Console.ForegroundColor = ConsoleColor.Red;
            // Console.Write("#");
            // Console.ForegroundColor = ConsoleColor.Gray;
            // Console.Write("#");
        }
    }
}