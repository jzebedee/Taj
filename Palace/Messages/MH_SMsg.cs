namespace Palace.Messages
{
    public class MH_SMsg : MH_Talk
    {
        public MH_SMsg(IPalaceConnection con, string msg)
            : base(con, msg)
        {
        }

        public MH_SMsg(IPalaceConnection con)
            : base(con)
        {
        }

        protected override MessageTypes MH_EventType
        {
            get { return MessageTypes.SMSG; }
        }
    }
}