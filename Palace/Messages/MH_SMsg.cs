using Palace.Messages.Structures;
namespace Palace.Messages
{
    public class MH_SMsg : MH_Talk
    {
        public MH_SMsg(string msg)
            : base(msg)
        {
        }

        public MH_SMsg(ClientMessage header, byte[] backing)
            : base(header, backing)
        {
        }

        protected override MessageTypes MH_EventType
        {
            get { return MessageTypes.SMSG; }
        }
    }
}