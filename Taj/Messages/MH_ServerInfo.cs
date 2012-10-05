using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;
using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_ServerInfo : MessageHeader
    {
        public MH_ServerInfo(PalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            Debug.WriteLine("MH_ServerInfo is unimplemented, and skipping itself ahead.");
            Reader.ReadBytes(cmsg.length);
        }
    }
}