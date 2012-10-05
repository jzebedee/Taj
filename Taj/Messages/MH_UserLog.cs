using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;
using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_UserLog : MessageHeader
    {
        public MH_UserLog(PalaceConnection con, ClientMessage cmsg) : base(con,cmsg)
        {
            var userID = cmsg.refNum;
            var numUsers = Reader.ReadInt32();

            Debug.WriteLine("MH_UserLog: {0} users, {1} joined", numUsers, userID);

            Palace.Users = numUsers;
        }
    }
}