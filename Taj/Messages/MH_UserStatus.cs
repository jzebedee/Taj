﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;
using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_UserStatus : MessageHeader
    {
        public MH_UserStatus(PalaceConnection con, ClientMessage cmsg) : base(con,cmsg)
        {
            Debug.WriteLine("MH_UserStatus is unimplemented, and skipping itself ahead.");
            Reader.ReadBytes(cmsg.length);
        }
    }
}