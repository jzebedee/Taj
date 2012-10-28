using System;
using System.Diagnostics;
using Taj.Messages.Flags;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    public class MH_ServerInfo : MessageHeader
    {
        public MH_ServerInfo(PalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            Palace.Permissions = (ServerPermissionsFlags)Reader.ReadUInt32();
            Palace.Name = Reader.ReadPString();

            //The server is shortchanging us?
            //var options = (ServerOptions)Reader.ReadUInt32();
            //var ulUploadCaps = Reader.ReadUInt32();
            //var ulDownloadCaps = Reader.ReadUInt32();
        }
    }
}