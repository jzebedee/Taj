using System.Diagnostics;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public class MH_UserLog : MessageReader
    {
        public MH_UserLog(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            NewUserID = cmsg.refNum;
            NewUserCount = Reader.ReadInt32();

            Debug.WriteLine("{0} users, {1} joined", NewUserCount, NewUserID);
        }

        public int NewUserID { get; private set; }
        public int NewUserCount { get; private set; }
    }
}