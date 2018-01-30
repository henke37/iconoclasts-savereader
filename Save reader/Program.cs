using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_reader {
    class Program {
        private const int APP_ID= 393520;

        static void Main(string[] args) {
            if(args.Length == 1) {
                DumpFile(args[0]);
            } else {
                string installPath=SteamHelper.GetInstallPathForApp(APP_ID);
                DumpFile($@"{installPath}\data\save1");
            }
        }

        private static void DumpFile(string fileName) {
            var fs = File.OpenRead(fileName);
            var sf = new SaveFile(fs);

            foreach(var kv in sf.entries) {
                Console.WriteLine($"{kv.Key} {kv.Value}");
            }
        }
    }
}
