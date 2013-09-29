using System;
using System.Diagnostics;
using Palace.Messages.Structures;
using System.Linq;

namespace Palace.Messages
{
    public class MH_Blowthru : MessageHeader, IOutgoingMessage
    {
        public byte[] Payload { get; private set; }

        public MH_Blowthru(IPalaceConnection con, ClientMessage cmsg)
            : base(con)
        {
            Payload = Reader.ReadBytes(cmsg.length);
            Debug.WriteLine("Blowthru (size {0}, refnum {1}): {2}", cmsg.length, cmsg.refNum, Payload.ToArrayString());
        }

        public void Write()
        {
            throw new NotImplementedException();
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