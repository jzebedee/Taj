using System.Diagnostics;
using Taj.Messages.Structures;
namespace Taj.Messages
{
    public class MH_Logoff : MessageHeader, IOutgoingMessage
    {
        public MH_Logoff(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            Palace.RemoveUserByID(cmsg.refNum);

            var UserCount = Reader.ReadInt32();
            Palace.UserCount = UserCount;

            Debug.WriteLine("Lost user {0}", cmsg.refNum);
            Debug.WriteLine("New user count: {0}", UserCount);
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