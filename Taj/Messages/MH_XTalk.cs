﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public class MH_XTalk : MessageHeader, IOutgoingMessage
    {
        public MH_XTalk(PalaceConnection con, string msg)
            : base(con)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg + '\0';
        }
        public MH_XTalk(PalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            var len = Reader.ReadInt16();
            byte[] xmsg = Reader.ReadBytes(len - 3);
            Reader.ReadByte();
            Text = PalaceEncryption.Decrypt(xmsg);
        }

        public string Text { get; private set; }

        public void Write()
        {
            var xmsg = PalaceEncryption.Encrypt(Text);

            Writer.WriteStruct(new ClientMessage
            {
                eventType = MessageTypes.MSG_XTALK,
                length = sizeof(short) + xmsg.Length,
                refNum = 0, //TODO: set refnum to userid
            });
            Writer.Write((short)xmsg.Length);
            Writer.Write(xmsg);
            Writer.Write((byte)0);
            Writer.Flush();
        }
    }
}
