using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages
{
    public class MH_PingPong : MessageHeader, IOutgoingMessage
    {
        readonly int _pingNum;

        public MH_PingPong(PalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            _pingNum = cmsg.refNum;
        }

        public void Write()
        {
            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.MSG_PONG,
                                       length = 0,
                                       refNum = _pingNum,
                                   });
            Writer.Flush();
        }
    }
}