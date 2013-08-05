using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Palace.Messages.Flags;

namespace Palace.Messages.Structures
{
    /// <summary>
    /// A clickable piece of screen real estate with a script that runs in response to various events
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HotspotRec
    {
        public static readonly int Size = Marshal.SizeOf(typeof(HotspotRec));

        [MarshalAs(UnmanagedType.I4)]
        public HotspotScriptEventMask scriptEventMask;
        public Int32 flags; //flags are various flag bits characterizing the hotspot <these are unused on the server>.
        public Int32 secureInfo; //secureInfo is a variable whose purpose in unclear. <It is not used.>
        public Int32 refCon; //refCon is an arbitrary use variable. <It is not used.>
        public Point loc; //the location of the hotspot
        public Int16 id;
        public Int16 dest;
        public Int16 nbrPts;
        public Int16 ptsOfst;
        public Int16 type;
        public Int16 groupID;
        public Int16 nbrScripts;
        public Int16 scriptRecOfst;
        public Int16 state;
        public Int16 nbrStates;
        public Int16 stateRecOfst;
        public Int16 nameOfst;
        public Int16 scriptTextOfst;
        public Int16 alignReserved; //x7
    }
}
