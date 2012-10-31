using System.Text;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    public class MH_Whisper : MessageHeader, IOutgoingMessage
    {
        public readonly PalaceUser Target;
        public readonly string Text;

        public MH_Whisper(PalaceConnection con, PalaceUser target, string msg)
            : base(con)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg;
            Target = target;
        }

        public MH_Whisper(PalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            Text = Reader.ReadCString();
            Target = Palace.GetUserByID(cmsg.refNum, true);
        }

        #region IOutgoingMessage Members

        public void Write()
        {
            byte[] msgBytes = Encoding.GetEncoding("Windows-1252").GetBytes(Text + '\0');

            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.WHISPER,
                                       length = sizeof (int) + msgBytes.Length,
                                       refNum = CurrentUser.ID,
                                   });
            Writer.Write(Target.ID);
            Writer.Write(msgBytes);
            Writer.Flush();
        }

        #endregion
    }
}