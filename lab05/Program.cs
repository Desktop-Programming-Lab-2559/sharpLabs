// Написать программу для перевода десятичных дробей в систему счисления с основанием 𝑘. Ваша программа должна уметь
// выделять периодическую часть получившейся дроби. Реализовать возможность ввода данных как с клавиатуры, так и из
// файла. Срок сдачи до 21 марта. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lab05
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string sNum;
            int baseNum;
            if (args.Length == 0)
            {
                Console.WriteLine("Enter number");
                sNum = Console.ReadLine();
                if (sNum == null) return;
                sNum = sNum.Replace('.', ',');
                if (!double.TryParse(sNum, out _))
                {
                    Console.WriteLine("Cannot parse number");
                    return;
                }

                Console.WriteLine("Enter base");
                var sBase = Console.ReadLine();
                if (sBase == null || !int.TryParse(sBase, out baseNum))
                {
                    Console.WriteLine("Cannot parse base");
                    return;
                }
            }
            else
            {
                var stream = File.Open(args[0], FileMode.Open);
                var arr = new byte[stream.Length];
                stream.Read(arr, 0, arr.Length);
                var text = System.Text.Encoding.Default.GetString(arr);
                
                sNum = text.Split(' ')[0];
                if (!double.TryParse(sNum, out _))
                {
                    Console.WriteLine("Cannot parse number");
                    return;
                }
                
                string sBase = text.Split(' ')[1];
                if (!int.TryParse(sBase, out baseNum))
                {
                    Console.WriteLine("Cannot parse base");
                    return;
                }
            }

            var tNum = int.Parse(sNum.Split(",")[0]);
            var before = tNum == 0 ? new StringBuilder("0") : new StringBuilder();
            while (tNum != 0)
            {
                Math.DivRem(tNum, baseNum, out var rem);
                var c = rem > 9 ? Convert.ToChar('A' + rem - 10).ToString() : rem.ToString();
                before.Insert(0, c);
                tNum /= baseNum;
            }

            var fractionPart = int.Parse(sNum.Split(',')[1]);
            List<int> rems = new List<int>(), fPartsList = new List<int>();
            var magic = (int) Math.Pow(10, sNum.Split(',')[1].Length);
            fPartsList.Add(fractionPart);
            fractionPart *= baseNum;

            while (fractionPart != 0 && !fPartsList.Contains(fractionPart % magic))
            {
                var rem = fractionPart / magic;
                rems.Add(rem);
                fractionPart -= rem * magic;
                fPartsList.Add(fractionPart);
                fractionPart *= baseNum;
            }

            if (fPartsList.Contains(fractionPart % magic) && fractionPart != 0) rems.Add(fractionPart / magic);

            Console.Write($"{before},");
            if (fractionPart == 0)
            {
                // rems.ForEach(Console.Write);
                // rem > 9 ? Convert.ToChar('A' + rem - 10).ToString() : rem.ToString()
                foreach (var rem in rems)
                {
                    Console.Write(rem > 9 ? Convert.ToChar('A' + rem - 10).ToString() : rem.ToString());
                }
                Console.WriteLine();
            }
            else
            {
                for (var i = 0; i < rems.Count; i++)
                {
                    if (i == fPartsList.IndexOf(fractionPart % magic)) Console.Write('(');
                    Console.Write(rems[i] > 9 ? Convert.ToChar('A' + rems[i] - 10).ToString() : rems[i].ToString());
                }
                Console.WriteLine(')');
            }
        }
    }
}