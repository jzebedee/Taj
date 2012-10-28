using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Taj.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct UserRec
    {
        public Int32 userID;
        public Point roomPos;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public AssetSpec[] propSpec; //[9]
        public Int16 roomID;
        public Int16 faceNbr;
        public Int16 colorNbr;
        public Int16 awayFlag;
        public Int16 openToMsgs;
        public Int16 nbrProps;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
        public byte[] name; //Str31
    }
}
