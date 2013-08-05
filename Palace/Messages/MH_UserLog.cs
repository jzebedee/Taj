using System.Diagnostics;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public class MH_UserLog : MessageHeader
    {
        public MH_UserLog(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            var NewUser = Palace.GetUserByID(cmsg.refNum, true);
            var UserCount = Reader.ReadInt32();

            Palace.UserCount = UserCount;

            Debug.WriteLine("{0} users, {1} joined", UserCount, NewUser);
        }
    }
}