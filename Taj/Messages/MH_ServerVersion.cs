using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public class MH_ServerVersion : MessageHeader
    {
        public MH_ServerVersion(PalaceConnection con, ClientMessage cmsg) : base(con)
        {
            short
                refVerLo = (short)(cmsg.refNum),
                refVerHi = (short)(cmsg.refNum >> 16);

            Version = new Version(refVerHi, refVerLo);
        }

        public readonly Version Version;
    }
}
