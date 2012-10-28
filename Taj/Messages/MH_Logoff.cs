using Taj.Messages.Structures;
namespace Taj.Messages
{
    public class MH_Logoff : MessageHeader, IOutgoingMessage
    {
        public PalaceUser LostUser { get; private set; }
        public int UserCount { get; private set; }

        public MH_Logoff(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            LostUser = Palace.GetUserByID(cmsg.refNum, true);
            Palace.RemoveUserByID(cmsg.refNum);

            UserCount = Reader.ReadInt32();
            Palace.UserCount = UserCount;
        }
        public MH_Logoff(IPalaceConnection con) : base(con) { }

        #region IOutgoingMessage Members

        public void Write()
        {
            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.LOGOFF,
                                       length = 0,
                                       refNum = 0, //intentional
                                   });
            Writer.Flush();
        }

        #endregion
    }
}