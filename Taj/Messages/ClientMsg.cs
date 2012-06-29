using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using MiscUtil.IO;

namespace Taj.Messages
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ClientMsg
    {
        public UInt32 eventType;
        public UInt32 length;
        public Int32 refNum;
        //public byte[] msg;

        //struct ClientMsg {
        //    uint32 eventType;   /* 32-bit opcode */
        //    uint32 length;      /* length of message body */
        //    sint32 refNum;      /* arbitrary integer operand */
        //    uint8  msg[length]; /* message body */
        //}

        public ClientMsg(EndianBinaryReader br)
        {
            eventType = br.ReadUInt32();
            length = br.ReadUInt32();
            refNum = br.ReadInt32();
            //msg = br.ReadBytes((int)length);
        }
    }
}
