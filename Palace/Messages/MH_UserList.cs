using System.Diagnostics;
using Palace.Messages.Structures;
using System.Collections.Generic;

namespace Palace.Messages
{
    public class MH_UserList : MessageReader
    {
        public MH_UserList(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            UserRecords = new Dictionary<int, UserRec>();

            for (int numUsers = cmsg.refNum; numUsers > 0; numUsers--)
            {
                var userRec = Reader.ReadStruct<UserRec>();

                UserRecords.Add(userRec.userID, userRec);
            }
        }

        public Dictionary<int, UserRec> UserRecords { get; private set; }
    }
}