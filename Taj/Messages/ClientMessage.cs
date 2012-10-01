using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using MiscUtil.IO;

namespace Taj.Messages
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ClientMessage
    {
        public UInt32 eventType;
        public Int32 length;
        public Int32 refNum;
    }

    //struct ClientMsg {
    //    uint32 eventType;   /* 32-bit opcode */
    //    uint32 length;      /* length of message body */
    //    sint32 refNum;      /* arbitrary integer operand */
    //    uint8  msg[length]; /* message body */
    //}
}