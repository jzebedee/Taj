using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Taj.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AssetQuery
    {
        public static readonly int Size = Marshal.SizeOf(typeof(AssetQuery));

        public AssetType type;
        public AssetSpec spec;
    }
}
