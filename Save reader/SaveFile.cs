using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_reader {
    class SaveFile {

        public Dictionary<string, dynamic> entries;

        private static readonly byte[] SIG = new byte[] { 0x4D, 0x41, 0x50, 0x31, 0x2E, 0x30 };


        public SaveFile(Stream s) {
            Read(s);
        }

        public SaveFile() {
            entries = new Dictionary<string, dynamic>();
        }

        public void Read(Stream s) {
            entries = new Dictionary<string, dynamic>();
            using(BinaryReader r = new BinaryReader(s)) {
                var sig = r.ReadBytes(SIG.Length);
                if(!sig.SequenceEqual(SIG)) {
                    throw new ArgumentException("Bad signature!");
                }

                uint entryCount = r.ReadUInt32();
                for(uint entryIndex = 0; entryIndex < entryCount; ++entryIndex) {
                    int keyLen = r.ReadInt32();
                    string key = r.ReadUTFString(keyLen);
                    uint type = r.ReadUInt32();
                    switch(type) {
                        case 1: {
                                var val = r.ReadDouble();
                                entries.Add(key, val);
                            }
                            break;
                        case 2: {
                                int valLen = r.ReadInt32();
                                string val = r.ReadUTFString(valLen);
                                entries.Add(key, val);
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        public void Write(Stream s) {
            using(BinaryWriter w = new BinaryWriter(s)) {
                w.Write(SIG);
                w.Write((int)entries.Count);
                foreach(var kv in entries) {
                    w.WriteLenPrefixedUTFString(kv.Key);
                    if(kv.Value is double) {
                        w.Write((int)1);
                        w.Write((double)kv.Value);
                    } else if(kv.Value is string) {
                        w.Write((int)2);
                        w.WriteLenPrefixedUTFString((string)kv.Value);
                    } else {
                        throw new NotImplementedException();
                    }
                }
            }
        }
    }
}
