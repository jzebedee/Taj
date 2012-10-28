using System.Diagnostics;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    public class MH_UserNew : MessageHeader
    {
        public MH_UserNew(IPalaceConnection con, ClientMessage cmsg) : base(con, cmsg)
        {
            Debug.WriteLine("MH_UserNew is unimplemented, and skipping itself ahead.");
            Reader.ReadBytes(cmsg.length);
        }
    }
}