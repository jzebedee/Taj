using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PictureRec
    {
        public static readonly int Size = Marshal.SizeOf(typeof(PictureRec));

        public Int32 refCon;
        public Int16 picID;
        public Int16 picNameOfst;
        public Int16 transColor;
        public Int16 reserved;
    }
}
