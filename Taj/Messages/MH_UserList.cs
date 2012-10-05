﻿using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_UserList : MessageHeader
    {
        public MH_UserList(PalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            Debug.WriteLine("MH_UserList is unimplemented, and skipping itself ahead.");
            Reader.ReadBytes(cmsg.length);
        }
    }
}