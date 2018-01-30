using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_reader {
    class Program {
        static void Main(string[] args) {
            var fs = File.OpenRead(args[0]);
            var sf = new SaveFile(fs);

            foreach(var kv in sf.entries) {
                Console.WriteLine($"{kv.Key} {kv.Value}");
            }
        }
    }
}
