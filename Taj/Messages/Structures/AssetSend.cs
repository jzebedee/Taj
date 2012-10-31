using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Taj.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AssetSend
    {
        public AssetType type;
        public AssetSpec spec;
        public Int32 blockSize;
        public Int32 blockOffset;
        public Int16 blockNbr;
        public Int16 nbrBlocks;
        public AssetDescriptor desc;
        //byte     data[blockSize];
    }
}
