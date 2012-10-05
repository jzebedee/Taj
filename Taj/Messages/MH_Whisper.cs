using System.Text;

namespace Taj.Messages
{
    public class MH_Whisper : MessageHeader, IOutgoingMessage
    {
        public readonly PalaceUser Target;

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
            Target = new PalaceUser {ID = cmsg.refNum};
        }

        public string Text { get; private set; }

        #region IOutgoingMessage Members

        public void Write()
        {
            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.MSG_WHISPER,
                                       length = sizeof (int) + Text.Length,
                                       refNum = Identity.ID, //TODO: set refnum to userid
                                   });
            Writer.Write(Target.ID);
            Writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(Text));
            Writer.Flush();
        }

        #endregion
    }
}