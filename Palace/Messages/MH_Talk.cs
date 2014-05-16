using System.Text;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public class MH_Talk : MessageWriter
    {
        public MH_Talk(string msg)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg;
        }

        public MH_Talk(ClientMessage header, byte[] backing)
            : base(header, backing)
        {
            Text = Reader.ReadCString();
        }

        protected virtual MessageTypes MH_EventType
        {
            get { return MessageTypes.TALK; }
        }

        public string Text { get; private set; }

        #region IOutgoingMessage Members

        public byte[] Write(int myID)
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(Text + '\0');

            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MH_EventType,
                                       length = msgBytes.Length,
                                       refNum = myID,
                                   });
            Writer.Write(msgBytes);
            Writer.Flush();

            return base.Write();
        }

        #endregion
    }
}