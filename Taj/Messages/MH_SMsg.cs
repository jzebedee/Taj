using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public class MH_SMsg : MH_Talk
    {
        protected override uint MH_EventType { get { return MessageTypes.MSG_SMSG; } }

        public MH_SMsg(PalaceConnection con, string msg)
            : base(con,msg)
        {
        }
        public MH_SMsg(PalaceConnection con)
            : base(con)
        {
        }
    }
}
