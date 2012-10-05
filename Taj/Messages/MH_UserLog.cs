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

            //TODO: implement
        }
    }
}