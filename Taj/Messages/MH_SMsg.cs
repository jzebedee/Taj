﻿namespace Taj.Messages
{
    public class MH_SMsg : MH_Talk
    {
        public MH_SMsg(PalaceConnection con, string msg)
            : base(con, msg)
        {
        }

        public MH_SMsg(PalaceConnection con)
            : base(con)
        {
        }

        protected override MessageTypes MH_EventType
        {
            get { return MessageTypes.SMSG; }
        }
    }
}