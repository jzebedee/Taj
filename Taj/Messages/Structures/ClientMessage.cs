﻿using System;
using System.Runtime.InteropServices;

namespace Taj.Messages.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ClientMessage
    {
        [MarshalAs(UnmanagedType.U4)]
        public MessageTypes eventType;
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