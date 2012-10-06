using System;
using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_ServerInfo : MessageHeader
    {
        public MH_ServerInfo(PalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            Palace.Permissions = (ServerPermissions)Reader.ReadUInt32();
            Palace.Name = Reader.ReadPString();

            //The server is shortchanging us?
            //var options = (ServerOptions)Reader.ReadUInt32();
            //var ulUploadCaps = Reader.ReadUInt32();
            //var ulDownloadCaps = Reader.ReadUInt32();
        }
    }
}