using System.Text;
using Palace.Messages.Structures;
using System.Diagnostics;

namespace Palace.Messages
{
    public class MH_Whisper : MessageWriter
    {
        public MH_Whisper(PalaceUser target, string msg)
        {
            Trace.Assert(msg.Length <= 255);

            Text = msg;
            TargetID = target.ID;
        }

        public MH_Whisper(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            Text = Reader.ReadCString();
            TargetID = cmsg.refNum;
        }

        public string Text { get; private set; }
        public int TargetID { get; private set; }

        #region IOutgoingMessage Members

        public byte[] Write(int myID)
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(Text + '\0');

            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.WHISPER,
                                       length = sizeof (int) + msgBytes.Length,
                                       refNum = myID,
                                   });
            Writer.Write(TargetID);
            Writer.Write(msgBytes);
            Writer.Flush();

            return base.Write();
        }

        #endregion
    }
}