using System;
using System.Diagnostics;
using Palace.Messages.Structures;
using System.Linq;

namespace Palace.Messages
{
    public class MH_Blowthru : MessageHeader
    {
        public byte[] Payload { get; private set; }

        public MH_Blowthru(IPalaceConnection con, ClientMessage cmsg)
            : base(con)
        {
            Payload = Reader.ReadBytes(cmsg.length);
            Debug.WriteLine("Blowthru (size {0}, refnum {1}): {2}", cmsg.length, cmsg.refNum, Payload.ToArrayString());
        }
    }
}