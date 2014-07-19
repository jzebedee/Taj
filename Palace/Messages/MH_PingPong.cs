using Palace.Messages.Structures;
namespace Palace.Messages
{
    public class MH_PingPong : MessageWriter
    {
        private readonly int _pingNum;

        public MH_PingPong(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            _pingNum = cmsg.refNum;
        }

        #region IOutgoingMessage Members

        public override byte[] Write()
        {
            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.PONG,
                                       length = 0,
                                       refNum = _pingNum,
                                   });
            Writer.Flush();

            return base.Write();
        }

        #endregion
    }
}