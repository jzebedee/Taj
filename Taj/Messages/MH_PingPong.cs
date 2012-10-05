﻿namespace Taj.Messages
{
    public class MH_PingPong : MessageHeader, IOutgoingMessage
    {
        private readonly int _pingNum;

        public MH_PingPong(PalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            _pingNum = cmsg.refNum;
        }

        #region IOutgoingMessage Members

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

        #endregion
    }
}