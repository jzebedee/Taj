using System;
using System.Diagnostics;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    public class MH_ServerVersion : MessageHeader
    {
        public MH_ServerVersion(IPalaceConnection con, ClientMessage cmsg)
            : base(con)
        {
            short
                refVerLo = (short)(cmsg.refNum),
                refVerHi = (short)(cmsg.refNum >> 16);

            Palace.Version = new Version(refVerHi, refVerLo);
            Debug.WriteLine("v{0}", Palace.Version);
        }
    }
}