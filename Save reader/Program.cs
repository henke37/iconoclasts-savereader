using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_reader {
    class Program {
        private const int APP_ID = 393520;

        static void Main(string[] args) {
            if(args.Length > 0) {
                if(Path.GetExtension(args[0]) == ".txt") {
                    string path = null;
                    if(args.Length > 1) {
                        if(Int32.TryParse(args[1], out int slot)) {
                            string installPath = SteamHelper.GetInstallPathForApp(APP_ID);
                            path = $@"{installPath}\data\save{slot}";
                            Console.WriteLine($"Writing to slot {slot}");
                        } else {
                            path = args[1];
                            Console.WriteLine($"Writing to path {path}");
                        }
                    } else {
                        string installPath = SteamHelper.GetInstallPathForApp(APP_ID);
                        path = $@"{installPath}\data\save1";
                        Console.WriteLine($"Writing to slot 1");
                    }
                    WriteFile(args[0], path);
                } else {
                    if(Int32.TryParse(args[0], out int slot)) {
                        string installPath = SteamHelper.GetInstallPathForApp(APP_ID);
                        DumpFile($@"{installPath}\data\save{slot}");
                    } else {
                        DumpFile(args[0]);
                    }
                }
            } else {
                string installPath = SteamHelper.GetInstallPathForApp(APP_ID);
                DumpFile($@"{installPath}\data\save1");
            }
        }

        private static void DumpFile(string fileName) {
            var fs = File.OpenRead(fileName);
            var sf = new SaveFile(fs);

            foreach(var kv in sf.entries) {
                Console.Write(kv.Key);
                if(kv.Value is double) Console.Write(" double ");
                else if(kv.Value is string) Console.Write(" string ");
                else Console.Write(" unknown ");
                Console.WriteLine(kv.Value);
            }
        }

        private static void WriteFile(string fileName, string outputPath) {
            FileStream output = File.OpenWrite(outputPath);
            SaveFile sf = new SaveFile();

            char[] delim = new char[] { ' ' };

            foreach(string line in File.ReadLines(fileName)) {
                string[] data = line.Split(delim, 3);
                switch(data[1]) {
                    case "double":
                        sf.entries.Add(data[0], Double.Parse(data[2]));
                        break;
                    case "string":
                        sf.entries.Add(data[0], data[2]);
                        break;
                    default:
                        Console.WriteLine($"Can't write unknown type \"{data[1]}\" for field \"{data[0]}\"");
                        break;
                }
            }
            sf.Write(output);
        }

    }
}
