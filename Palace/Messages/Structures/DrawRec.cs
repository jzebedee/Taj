using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DrawRec
    {
        public static readonly int Size = Marshal.SizeOf(typeof(DrawRec));

        public LLRec link;
        public Int16 drawCmd;
        public UInt16 cmdLength;
        public Int16 dataOfst;
        //byte  cmdData[cmdLength];
    }
}
