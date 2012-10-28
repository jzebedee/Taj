using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Taj.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LPropRec
    {
        public static readonly int Size = Marshal.SizeOf(typeof(LPropRec));

        public LLRec link; //link is an LLRec struct, as described above. It is used internally and has no purpose when sent over wire 
        public AssetSpec propSpec; //propSpec identifies the asset that should be used to represent the prop on the client’s screen.
        public Int32 flags; //flags are various flag bits characterizing the prop, used on the client.
        public Int32 refCon; //refCon is an arbitrary use variable, used by the client.
        public Point loc; //loc is the screen location at which the prop should be displayed.
    }
}
