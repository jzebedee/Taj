using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public class MH_Whisper : MessageHeader, IOutgoingMessage
    {
        public MH_Whisper(PalaceConnection con, PalaceUser target, string msg)
            : base(con)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg + '\0';
            Target = target;
        }
        public MH_Whisper(PalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            Text = Reader.ReadCString();
            Target = new PalaceUser { ID = cmsg.refNum };
        }

        public string Text { get; private set; }
        public readonly PalaceUser Target;

        public void Write()
        {
            Writer.WriteStruct(new ClientMessage
            {
                eventType = MessageTypes.MSG_WHISPER,
                length = sizeof(int) + Text.Length,
                refNum = Identity.ID, //TODO: set refnum to userid
            });
            Writer.Write(Target.ID);
            Writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(Text));
            Writer.Flush();
        }
    }
}