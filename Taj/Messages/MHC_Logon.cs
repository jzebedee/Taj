using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using MiscUtil.IO;

namespace Taj.Messages
{
    public struct MHC_Logon : IMessageHandler
    {
        const uint
            guest_puidCtr = 0xf5dc385e,
            guest_puidCRC = 0xc144c580;

        AuxRegistrationRec rec;

        public MHC_Logon(string name, short desiredRoom = 0, uint puidCtr = guest_puidCtr, uint puidCRC = guest_puidCRC)
        {
            rec = new AuxRegistrationRec
            {
                crc = 0x5905f923,       //cribbed guest from OP
                counter = 0xcf07309c,   //cribbed guest from OP
                userName = name.ToStr31(),
                wizPassword = string.Empty.ToStr31(),
                auxFlags = 0x80000004,  //AUXFLAGS_AUTHENTICATE | AUXFLAGS_WIN32
                puidCtr = puidCtr,   //cribbed guest from OP
                puidCRC = puidCRC,   //cribbed guest from OP

                demoElapsed = 0,        //garbage
                totalElapsed = 0,       //garbage
                demoLimit = 0,          //garbage

                desiredRoom = desiredRoom,

                reserved = Encoding.GetEncoding("iso-8859-1").GetBytes("OPNPAL"),

                ulRequestedProtocolVersion = 0,

                ulUploadCaps = 0x1,     //ULCAPS_ASSETS_PALACE
                ulDownloadCaps = 0x111, //DLCAPS_ASSETS_PALACE | DLCAPS_FILES_PALACE | DLCAPS_FILES_HTTPSRVR
                ul2DEngineCaps = 0,
                ul2DGraphicsCaps = 0,
                ul3DEngineCaps = 0,
            };
        }

        public MHC_Logon(EndianBinaryReader reader)
        {
            rec = reader.ReadStruct<AuxRegistrationRec>();
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.WriteStruct(new ClientMessage
                {
                    eventType = MessageTypes.MSG_LOGON,
                    length = 128,
                    refNum = 0, //intentional
                });
            writer.WriteStruct(rec);

            writer.Flush();
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 128)]
    struct AuxRegistrationRec
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
