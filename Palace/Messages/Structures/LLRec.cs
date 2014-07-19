using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LLRec
    {
        public Int16 nextOfst;
        public Int16 reserved;
    }
}
