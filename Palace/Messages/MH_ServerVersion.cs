using System;
using System.Diagnostics;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public class MH_ServerVersion : MessageWriter
    {
        public MH_ServerVersion(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            short
                refVerLo = (short)(cmsg.refNum),
                refVerHi = (short)(cmsg.refNum >> 16);

            Version = new Version(refVerHi, refVerLo);
            Debug.WriteLine("v{0}", Version);
        }
        public MH_ServerVersion(Version v)
        {
            Version = v;
        }

        public Version Version { get; private set; }

        public override byte[] Write()
        {
            var head = new ClientMessage
            {
                eventType = MessageTypes.VERSION,
                length = 0,
                refNum = Version.Minor + Version.Major << 16
            };
            Writer.WriteStruct(head);

            return base.Write();
        }
    }
}