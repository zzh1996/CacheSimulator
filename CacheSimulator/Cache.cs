using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CacheSimulator
{
    class Cache
    {
        public int ReadICount, ReadIMiss, ReadDCount, ReadDMiss, WriteCount, WriteMiss;
        public int CacheSize, LineSize, Associativity, ReplaceMethod, PrefetchMethod, WriteMethod;

        public void Reset()
        {
            ReadICount = ReadIMiss = ReadDCount = ReadDMiss = WriteCount = WriteMiss = 0;
        }

        public void RunOnce(int type, UInt32 address)
        {
            switch (type)
            {
                case 0: //read data
                    ReadDCount++;
                    break;
                case 1: //write data
                    WriteCount++;
                    break;
                case 2: //read instruction
                    ReadICount++;
                    break;
            }
        }
    }
}
