using Taj.Messages.Structures;
namespace Taj.Messages
{
    public class MH_XTalk : MessageHeader, IOutgoingMessage
    {
        public MH_XTalk(IPalaceConnection con, string msg)
            : base(con)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg;
        }

        public MH_XTalk(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            short len = Reader.ReadInt16();
            byte[] xmsg = Reader.ReadBytes(len - 3);
            Reader.ReadByte();
            Text = PalaceEncryption.Decrypt(xmsg);
        }

        public string Text { get; private set; }

        #region IOutgoingMessage Members

        public void Write()
        {
            byte[] xmsg = PalaceEncryption.Encrypt(Text);

            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.XTALK,
                                       length = sizeof (short) + xmsg.Length + 1,
                                       refNum = CurrentUser.ID,
                                   });
            Writer.Write((short) (xmsg.Length + 3));
            Writer.Write(xmsg);
            Writer.Write((byte) 0);
            Writer.Flush();
        }

        #endregion
    }
}