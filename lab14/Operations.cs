using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace lab14
{
    public class Operations : IEnumerable
    {
        private List<Operation> _operations { get; set; } = new List<Operation>();
        
        public Operations(){}
        
        public static Operations Load(string path)
        {
            if (File.Exists(path))
            {
                // var b = File.ReadAllBytes(path);
                // var reader = new Utf8JsonReader(b);
                // return JsonSerializer.Deserialize<Operations>(ref reader);
                return JsonSerializer.Deserialize<Operations>(File.ReadAllText(path));
            }
            throw new ArgumentException("File doesn't exists");
        }

        public void Save(string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            File.WriteAllText(path, JsonSerializer.Serialize(this, options));
            // File.WriteAllBytes(path, JsonSerializer.SerializeToUtf8Bytes(this, options));
        }

        public void Add(Operation op)
        {
            _operations.Add(op);
        }

        public IEnumerator GetEnumerator()
        {
            return _operations.GetEnumerator();
        }
    }
}