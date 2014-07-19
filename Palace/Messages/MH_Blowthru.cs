using System;
using System.Diagnostics;
using Palace.Messages.Structures;
using System.Linq;

namespace Palace.Messages
{
    public class MH_Blowthru : MessageWriter
    {
        public byte[] Payload { get; private set; }

        public MH_Blowthru(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            Payload = Reader.ReadBytes(cmsg.length);
            Debug.WriteLine("Blowthru (size {0}, refnum {1}): {2}", cmsg.length, cmsg.refNum, Payload.ToArrayString());
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
            return base.Write();
            //if (sploit)
            //{
            //    Writer.WriteStruct(new ClientMessage
            //    {
            //        eventType = MessageTypes.BLOWTHRU,
            //        length = int.MaxValue,
            //        refNum = int.MinValue,
            //    });

            //    var rand = new Random();
            //    for (int i = 0; i < 10; i++)
            //    {
            //        var buf = new byte[1024];
            //        rand.NextBytes(buf);
            //        Writer.Write(buf);
            //    }
            //}

            //Writer.Flush();
        }
    }
}