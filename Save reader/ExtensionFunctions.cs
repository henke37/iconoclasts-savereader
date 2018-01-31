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

        public static string ReadAsUTF8(this Stream s) {
            var ba = new byte[s.Length];
            s.Read(ba, 0, (int)s.Length);
            return new string(Encoding.UTF8.GetChars(ba));
        }

        public static void Skip(this BinaryReader r, int bytesToSkip) {
            r.BaseStream.Seek(bytesToSkip, SeekOrigin.Current);
        }

        public static void WriteLenPrefixedUTFString(this BinaryWriter w,string str) {
            var ba=Encoding.UTF8.GetBytes(str+"\0");
            w.Write((int)ba.Length);
            w.Write(ba);
        }
    }
}
