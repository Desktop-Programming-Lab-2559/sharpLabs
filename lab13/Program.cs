// Напишите приложение для поиска всех файлов с заданным именем на компьютере. Приложение должно показать путь
// к найденным файлам. Реализуйте возможность просмотра найденных файлов в текстовом окне и опцию сжатия с
// использованием объекта класса GZipStream. Срок сдачи до 16 мая. 

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace lab13
{
    internal class Program
    {
        private const string Notepad = "C:/Windows/System32/notepad.exe";

        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments");
                return;
            }

            var baseDirectory = args[0];
            var fileName = args[1];

            var fileCollection = new StringCollection();
            SearchFile(baseDirectory, fileName, fileCollection);

            for (var i = 1; i < fileCollection.Count; i++) Console.WriteLine($"[{i.ToString().PadLeft(3)}] {fileCollection[i]}");


            Console.WriteLine("Enter file index");
            var s = Console.ReadLine();
            if (int.TryParse(s, out var index))
            {
                if (index > fileCollection.Count || index < 1)
                {
                    Console.WriteLine("err");
                    return;
                }
            }
            else
            {
                Console.WriteLine("err");
                return;
            }

            var p = OpenFile(fileCollection[index]);

            Compress(fileCollection[index], $"{fileCollection[index]}.gz");
            Console.WriteLine($"{fileCollection[index]}.gz");
            
            Console.ReadKey();
            try
            {
                p?.Kill();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't close notepad\n{e.Message}");
            }
        }

        public static Process OpenFile(string file)
        {
            Process process = null;
            try
            {
                process = Process.Start(Notepad, file);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cannot open file\n{e.Message}");
                throw;
            }

            return process;
        }

        public static void SearchFile(string directory, string pattern, StringCollection collection)
        {
            try
            {
                foreach (var file in Directory.GetFiles(directory, pattern)) collection.Add(file);
            }
            catch (UnauthorizedAccessException e)
            {
                // Console.WriteLine(e);
                return;
            }

            foreach (var childDirectory in Directory.EnumerateDirectories(directory))
                SearchFile(childDirectory, pattern, collection);
        }

        public static void Compress(string sourceFile, string compressedFile)
        {
            using (var sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                using (var targetStream = File.Create(compressedFile))
                {
                    using (var compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }
    }
}