using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KVLib;
using Microsoft.Win32;

namespace Save_reader {
    class SteamHelper {

        static readonly List<string> libraryFolders;

        static SteamHelper() {
            libraryFolders = FindLibraries();
        }

        static dynamic FindLibraries() {
            var steamKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Valve\Steam");
            string steamInstallPath = (string)steamKey.GetValue("InstallPath");

            var libraries = new List<string>();

            libraries.Add(steamInstallPath);

            var kv = parseKVFile($@"{steamInstallPath}\steamapps\libraryfolders.vdf");

            
            for(uint libraryId = 1; ; ++libraryId) {
                var folder = kv[libraryId.ToString()];
                if(folder == null) break;
                libraries.Add(fixSlashes(folder.GetString()));
            }

            return libraries;
        }

        private static string fixSlashes(string v) {
            return Regex.Replace(v, @"\\\\", @"\");
        }

        internal static string GetInstallPathForApp(int appId) {
            foreach(var libraryFolder in libraryFolders) {
                try {
                    KeyValue manifest = parseKVFile($@"{libraryFolder}\steamapps\appmanifest_{appId}.acf");
                    string installDir = fixSlashes(manifest["installdir"].GetString());
                    return $@"{libraryFolder}\steamapps\common\{installDir}";
                } catch(FileNotFoundException) {
                    continue;
                }
            }
            throw new KeyNotFoundException();
        }

        private static KeyValue parseKVFile(string filename) {
            return parseKVStream(File.OpenRead(filename));
        }

        private static KeyValue parseKVStream(Stream s) {
            return KVParser.ParseAllKVRootNodes(s.ReadAsUTF8())[0];
        }
    }
}
