using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    public class MH_RoomDesc : MessageHeader
    {
        public MH_RoomDesc(IPalaceConnection con, ClientMessage cmsg) : base(con, cmsg)
        {
            //var tbytes = Reader.ReadBytes(cmsg.length);
            //using (var writer = new BinaryWriter(File.OpenWrite("roomdesc.dump")))
            //    writer.Write(tbytes);
            //return;

            int totalread = 0;
            try
            {
                var roomrec = Reader.ReadStruct<RoomRec>();
                totalread += Marshal.SizeOf(typeof(RoomRec));
                var varbuf = Reader.ReadBytes(roomrec.lenVars);
                totalread += roomrec.lenVars;
                Debug.WriteLine(roomrec);
                Debug.WriteLine(varbuf);
                var finalread = Reader.ReadInt32();
                totalread += sizeof(Int32);
            }
            finally
            {
                int remaining = cmsg.length - totalread;
                if (remaining > 0)
                {
                    var leftovers = Reader.ReadBytes(remaining);
                    Debug.WriteLine(leftovers);
                }
            }
        }
    }
}