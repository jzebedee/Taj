using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Taj.Messages
{
    public class ClientMsg
    {
        //struct ClientMsg {
        //    uint32 eventType;   /* 32-bit opcode */
        //    uint32 length;      /* length of message body */
        //    sint32 refNum;      /* arbitrary integer operand */
        //    uint8  msg[length]; /* message body */
        //}

        public ClientMsg(BinaryReader br)
        {
            eventType = br.ReadUInt32();
            length = br.ReadUInt32();
            refNum = br.ReadInt32();

            msg = new byte[length];
            br.Read(msg, 0, (int)length);
        }

        public UInt32 eventType { get; private set; }
        public UInt32 length { get; private set; }
        public Int32 refNum { get; private set; }
        public byte[] msg { get; private set; }

        public int Size
        {
            get
            {
                return sizeof(UInt32) * 2 + sizeof(Int32) * 1 + msg.Length;
            }
        }
    }
}
