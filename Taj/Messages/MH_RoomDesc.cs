using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_RoomDesc : MessageHeader
    {
        public MH_RoomDesc(IPalaceConnection con, ClientMessage cmsg) : base(con, cmsg)
        {
            Debug.WriteLine("MH_RoomDesc is unimplemented, and skipping itself ahead.");
            Reader.ReadBytes(cmsg.length);
        }
    }
}