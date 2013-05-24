using System;
using System.Runtime.InteropServices;
using Taj.Messages.Flags;

namespace Taj.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 128)]
    public struct AuxRegistrationRec
    {
        public UInt32 crc;
        public UInt32 counter;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
        public byte[] userName; //Str31

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
        public byte[] wizPassword; //Str31

        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public AuxFlags auxFlags;
        public UInt32 puidCtr;
        public UInt32 puidCRC;
        public UInt32 demoElapsed;
        public UInt32 totalElapsed;
        public UInt32 demoLimit;
        public Int16 desiredRoom;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
        public byte[] reserved; //len: 6

        public UInt32 ulRequestedProtocolVersion;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public ulUploadCapsFlags ulUploadCaps;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public ulDownloadCapsFlags ulDownloadCaps;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public ul2DEngineCapsFlags ul2DEngineCaps;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public ul2DGraphicsCapsFlags ul2DGraphicsCaps;
        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public ul3DEngineCapsFlags ul3DEngineCaps;
    }
}