﻿using System.Diagnostics;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public class MH_UserList : MessageHeader
    {
        public MH_UserList(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            for (int numUsers = cmsg.refNum; numUsers > 0; numUsers--)
            {
                var userRec = Reader.ReadStruct<UserRec>();
                var user = Palace.GetUserByID(userRec.userID, true);

                userRec.Populate(user);
            }
        }
    }
}