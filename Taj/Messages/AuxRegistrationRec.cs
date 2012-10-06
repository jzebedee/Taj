using System;
using System.Runtime.InteropServices;

namespace Taj.Messages
{
    [StructLayout(LayoutKind.Sequential, Size = 128)]
    internal struct AuxRegistrationRec
    {
        public UInt32 crc;
        public UInt32 counter;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
        public byte[] userName; //Str31

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
        public byte[] wizPassword; //Str31

        public UInt32 auxFlags;
        public UInt32 puidCtr;
        public UInt32 puidCRC;
        public UInt32 demoElapsed;
        public UInt32 totalElapsed;
        public UInt32 demoLimit;
        public Int16 desiredRoom;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
        public byte[] reserved; //len: 6

        public UInt32 ulRequestedProtocolVersion;
        public UInt32 ulUploadCaps;
        public UInt32 ulDownloadCaps;
        public UInt32 ul2DEngineCaps;
        public UInt32 ul2DGraphicsCaps;
        public UInt32 ul3DEngineCaps;
    }
}