using System.Text;

namespace Taj.Messages
{
    public class MH_Talk : MessageHeader, IOutgoingMessage
    {
        public MH_Talk(PalaceConnection con, string msg)
            : base(con)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg;
        }

        public MH_Talk(PalaceConnection con)
            : base(con)
        {
            Text = Reader.ReadCString();
        }

        protected virtual uint MH_EventType
        {
            get { return MessageTypes.MSG_TALK; }
        }

        public string Text { get; private set; }

        #region IOutgoingMessage Members

        public void Write()
        {
            byte[] msgBytes = Encoding.GetEncoding("Windows-1252").GetBytes(Text + '\0');

            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MH_EventType,
                                       length = msgBytes.Length,
                                       refNum = 0, //TODO: set refnum to userid
                                   });
            Writer.Write(msgBytes);
            Writer.Flush();
        }

        #endregion
    }
}