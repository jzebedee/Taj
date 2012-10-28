﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Taj.Messages.Flags;

namespace Taj.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RoomRec
    {
        [MarshalAs(UnmanagedType.I4)]
        public RoomFlags roomFlags;
        public Int32 facesID;
        public Int16 roomID;
        public Int16 roomNameOfst;
        public Int16 pictNameOfst;
        public Int16 artistNameOfst;
        public Int16 passwordOfst;
        public Int16 nbrHotspots;
        public Int16 hotspotOfst;
        public Int16 nbrPictures;
        public Int16 pictureOfst;
        public Int16 nbrDrawCmds;
        public Int16 firstDrawCmd;
        public Int16 nbrPeople;
        public Int16 nbrLProps;
        public Int16 firstLProp;
        public Int16 reserved; //reserved is a filler block to maintain field alignment. It should be set to 0 .
        public Int16 lenVars;
        //byte  varBuf[lenVars];
    }
}
