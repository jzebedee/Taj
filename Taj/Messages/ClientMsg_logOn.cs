using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Taj.Messages
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ClientMsg_logOn
    {
        public ClientMsg_logOn(string name)
        {
            msg = new ClientMsg
            {
                eventType = MessageTypes.Logon,
                length = 128,
                refNum = 0 //intentional
            };
            rec = new AuxRegistrationRec
            {
                crc = 0x5905f923,       //cribbed guest from OP
                counter = 0xcf07309c,   //cribbed guest from OP
                userName = new Str31 { Content = name },
                wizPassword = new Str31 { },
                auxFlags = 0x80000004,  //AUXFLAGS_AUTHENTICATE | AUXFLAGS_WIN32
                puidCtr = 0xf5dc385e,   //cribbed guest from OP
                puidCRC = 0xc144c580,   //cribbed guest from OP

                demoElapsed = 0,    //garbage
                totalElapsed = 0,   //garbage
                demoLimit = 0,      //garbage

                desiredRoom = 0,

                reserved = null,

                ulRequestedProtocolVersion = 0,
                ulUploadCaps = 0x1,     //ULCAPS_ASSETS_PALACE
                ulDownloadCaps = 0x111, //DLCAPS_ASSETS_PALACE | DLCAPS_FILES_PALACE | DLCAPS_FILES_HTTPSRVR
                ul2DEngineCaps = 0,
                ul2DGraphicsCaps = 0,
                ul3DEngineCaps = 0,
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
        //public Int32 auxFlags;
        public UInt32 auxFlags;
        public UInt32 puidCtr;
        public UInt32 puidCRC;
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
