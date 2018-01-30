using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_reader {
    static class ExtensionFunctions {
        public static string ReadUTFString(this BinaryReader r,int bytesToRead) {
            var ba = r.ReadBytes(bytesToRead);
            return new String(Encoding.UTF8.GetChars(ba,0,bytesToRead-1));
        }

        public static void Skip(this BinaryReader r, int bytesToSkip) {
            r.BaseStream.Seek(bytesToSkip, SeekOrigin.Current);
        }
    }
}
