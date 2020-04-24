using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace lab14
{
    public class Test
    {
        private List<int> _list { get; set; } = new List<int>() {1, 2, 3, 4, 5};

        public void Save()
        {
            File.WriteAllText(@"C:\Users\viktor\RiderProjects\Labs\lab14\testTest.txt", JsonSerializer.Serialize(this));
        }

        public static Test Load()
        {
            return JsonSerializer.Deserialize<Test>(
                File.ReadAllText(@"C:\Users\viktor\RiderProjects\Labs\lab14\testTest.txt"));
        }

        public override string ToString()
        {
            return _list.Select(x => x.ToString()).Aggregate((s, s1) => s + s1);
        }
    }
}