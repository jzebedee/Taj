using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Taj.Messages.Structures
{
    /// <summary>
    /// Information about the asset as a whole
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AssetDescriptor
    {
        public UInt32 flags;
        public UInt32 size;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
        public byte[] name; //Str31
    }
}
