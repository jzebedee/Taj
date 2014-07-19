using Palace.Messages.Structures;
namespace Palace.Messages
{
    public class MH_XTalk : MessageWriter
    {
        public MH_XTalk(string msg)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg;
        }

        public MH_XTalk(ClientMessage cmsg, byte[] payload)
            : base(cmsg, payload)
        {
            short len = Reader.ReadInt16();
            byte[] xmsg = Reader.ReadBytes(len - 3);
            Reader.ReadByte();
            Text = PalaceEncryption.Decrypt(xmsg);
        }

        public string Text { get; private set; }

        #region IOutgoingMessage Members

        public byte[] Write(int myID)
        {
            byte[] xmsg = PalaceEncryption.Encrypt(Text);

            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.XTALK,
                                       length = sizeof(short) + xmsg.Length + 1,
                                       refNum = myID,
                                   });
            Writer.Write((short)(xmsg.Length + 3));
            Writer.Write(xmsg);
            Writer.Write((byte)0);
            Writer.Flush();

            return base.Write();
        }

        #endregion
    }
}