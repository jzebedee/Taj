using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Taj.Messages
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG_LOGON
    {
        public MSG_LOGON(int x)
        {
            msg = new ClientMsg
            {
                eventType = MessageTypes.Outgoing_Logon,
                length = 128,
                refNum = 0 //intentional
            };
            rec = new AuxRegistrationRec
            {
                crc = 0x5905f923, //cribbed guest from OP
                counter = 0xcf07309c, //cribbed guest from OP
                userName = new Str31 { Content = "Hello" }
            };
        }

        ClientMsg msg;
        AuxRegistrationRec rec;
    }

    [StructLayout(LayoutKind.Sequential, Size = 128)]
    unsafe struct AuxRegistrationRec
    {
        public UInt32 crc;
        public UInt32 counter;
        public Str31 userName;
        public Str31 wizPassword;
        public Int32 auxFlags;
        public UInt32 puidCtr;
        public UInt32 CRC;
        public UInt32 demoElapsed;
        public UInt32 totalElapsed;
        public UInt32 demoLimit;
        public Int16 desiredRoom;
        public fixed byte reserved[6];
        public UInt32 ulRequestedProtocolVersion;
        public UInt32 ulUploadCaps;
        public UInt32 ulDownloadCaps;
        public UInt32 ul2DEngineCaps;
        public UInt32 ul2DGraphicsCaps;
        public UInt32 ul3DEngineCaps;
    }
}
