using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CacheSimulator
{
    class Block
    {
        private int Tag;
        public int LastAccessTime;
        public int LastReplaceTime;

        public Block()
        {
            Tag = -1;
            LastAccessTime = -1;
            LastReplaceTime = -1;
        }

        public bool Access(int BlockNum, int cycle)
        {
            if (BlockNum == Tag)
            {
                LastAccessTime = cycle;
                return true;
            }
            return false;
        }

        public void Replace(int NewTag, int cycle)
        {
            Tag = NewTag;
            LastReplaceTime = LastAccessTime = cycle;
        }
    }

    class Index
    {
        private Block[] blocks;

        public Index()
        {
            blocks = new Block[32];
            for (int i = 0; i < 32; i++)
            {
                blocks[i] = new Block();
            }
        }

        public bool Search(int BlockNum, int cycle)
        {
            for (int i = 0; i < Cache.Associativity; i++)
            {
                if (blocks[i].Access(BlockNum, cycle))
                {
                    return true;
                }
            }
            return false;
        }

        public void Insert(int BlockNum, int cycle)
        {
            int MinTime, MinBlock;
            switch (Cache.ReplaceMethod)
            {
                case 0: //LRU
                    MinTime = blocks[0].LastAccessTime;
                    MinBlock = 0;
                    for (int i = 0; i < Cache.Associativity; i++)
                    {
                        if (blocks[i].LastAccessTime < MinTime)
                        {
                            MinTime = blocks[i].LastAccessTime;
                            MinBlock = i;
                        }
                    }
                    blocks[MinBlock].Replace(BlockNum, cycle);
                    break;
                case 1: //FIFO
                    MinTime = blocks[0].LastReplaceTime;
                    MinBlock = 0;
                    for (int i = 0; i < Cache.Associativity; i++)
                    {
                        if (blocks[i].LastReplaceTime < MinTime)
                        {
                            MinTime = blocks[i].LastReplaceTime;
                            MinBlock = i;
                        }
                    }
                    blocks[MinBlock].Replace(BlockNum, cycle);
                    break;
                case 2: //RAND
                    blocks[Cache.rnd.Next(0, Cache.Associativity)].Replace(BlockNum, cycle);
                    break;
            }
        }
    }

    class Cache
    {
        public int ReadICount, ReadIMiss, ReadDCount, ReadDMiss, WriteCount, WriteMiss;
        public static int CacheSize, BlockSize, Associativity, ReplaceMethod, PrefetchMethod, WriteMethod;
        public UInt32 Address;
        public int Type, BlockNum, InBlockAddress, IndexNum;
        public bool Miss, Updated;
        private Index[] indexes;
        private int cycle;
        public static Random rnd;

        public void Reset()
        {
            ReadICount = ReadIMiss = ReadDCount = ReadDMiss = WriteCount = WriteMiss = 0;
            Updated = false;
            indexes = new Index[65536];
            for (int i = 0; i < 65536; i++)
            {
                indexes[i] = new Index();
            }
            cycle = 0;
            rnd = new Random();
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
            if (Visit(address, type))
            {
                switch (type)
                {
                    case 0: //read data
                        ReadDMiss++;
                        break;
                    case 1: //write data
                        WriteMiss++;
                        break;
                    case 2: //read instruction
                        ReadIMiss++;
                        break;
                }
            }
            Type = type;
            Address = address;
            Updated = true;
        }

        private bool Visit(UInt32 address, int type)
        {
            BlockNum = (int)(address / BlockSize);
            InBlockAddress = (int)(address % BlockSize);
            int IndexCount = CacheSize / BlockSize / Associativity;
            if (IndexCount == 0)
            {
                MessageBox.Show("mdzz!");
                return false;
            }
            IndexNum = BlockNum % IndexCount;
            Miss = !indexes[IndexNum].Search(BlockNum, cycle);
            if (Miss && (type != 1 || type == 1 && WriteMethod == 0))
            {
                indexes[IndexNum].Insert(BlockNum, cycle);
            }
            cycle++;
            return Miss;
        }
    }
}
