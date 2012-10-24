using System;

namespace Taj.Messages
{
    public class MH_ServerVersion : MessageHeader
    {
        public readonly Version Version;

        public MH_ServerVersion(IPalaceConnection con, ClientMessage cmsg) : base(con)
        {
            short
                refVerLo = (short) (cmsg.refNum),
                refVerHi = (short) (cmsg.refNum >> 16);

            Version = new Version(refVerHi, refVerLo);
        }
    }
}