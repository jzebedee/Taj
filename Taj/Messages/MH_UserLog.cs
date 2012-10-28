using System.Diagnostics;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    public class MH_UserLog : MessageHeader
    {
        public int UserCount { get; private set; }
        public PalaceUser NewUser { get; private set; }

        public MH_UserLog(IPalaceConnection con, ClientMessage cmsg) : base(con, cmsg)
        {
            NewUser = Palace.GetUserByID(cmsg.refNum, true);
            UserCount = Reader.ReadInt32();

            Palace.UserCount = UserCount;
        }
    }
}