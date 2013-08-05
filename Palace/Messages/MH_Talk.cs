using System.Text;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public class MH_Talk : MessageHeader, IOutgoingMessage
    {
        public MH_Talk(IPalaceConnection con, string msg)
            : base(con)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg;
        }

        public MH_Talk(IPalaceConnection con)
            : base(con)
        {
            Text = Reader.ReadCString();
        }

        protected virtual MessageTypes MH_EventType
        {
            get { return MessageTypes.TALK; }
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
                                       refNum = CurrentUser.ID,
                                   });
            Writer.Write(msgBytes);
            Writer.Flush();
        }

        #endregion
    }
}