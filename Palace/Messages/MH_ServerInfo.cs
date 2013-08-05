using System;
using System.Diagnostics;
using Palace.Messages.Flags;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public class MH_ServerInfo : MessageHeader
    {
        public MH_ServerInfo(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            Palace.Permissions = (ServerPermissionsFlags)Reader.ReadUInt32();
            Palace.Name = Reader.ReadPString();

            //The server is shortchanging us?
            //var options = (ServerOptions)Reader.ReadUInt32();
            //var ulUploadCaps = Reader.ReadUInt32();
            //var ulDownloadCaps = Reader.ReadUInt32();

            Debug.WriteLine("Name: " + Palace.Name);
            Debug.WriteLine("Permissions: " + Palace.Permissions);
        }
    }
}