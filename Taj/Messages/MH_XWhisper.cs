using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public class MH_XWhisper : MessageHeader, IOutgoingMessage
    {
        public MH_XWhisper(PalaceConnection con, PalaceUser target, string msg) : base(con)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg + '\0';
            Target = target;
        }
        public MH_XWhisper(PalaceConnection con, ClientMessage cmsg) : base(con,cmsg)
        {
            var len = Reader.ReadInt16();
            byte[] xmsg = Reader.ReadBytes(len - 3);
            Reader.ReadByte();
            Text = PalaceEncryption.Decrypt(xmsg);

            Target = new PalaceUser { ID = cmsg.refNum };
        }

        public string Text { get; private set; }
        public readonly PalaceUser Target;

        public void Write()
        {
            var xmsg = PalaceEncryption.Encrypt(Text);

            Writer.WriteStruct(new ClientMessage
            {
                eventType = MessageTypes.MSG_XWHISPER,
                length = sizeof(int) + sizeof(short) + xmsg.Length,
                refNum = Identity.ID, //TODO: set refnum to userid
            });
            Writer.Write(Target.ID);
            Writer.Write((short)xmsg.Length);
            Writer.Write(xmsg);
            Writer.Write((byte)0);
            Writer.Flush();
        }
    }
}