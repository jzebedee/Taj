using System;
using System.Diagnostics;
using Palace.Messages.Flags;
using Palace.Messages.Structures;
using System.Text;

namespace Palace.Messages
{
    public class MH_ServerInfo : MessageWriter
    {
        public MH_ServerInfo(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            Permissions = (ServerPermissionsFlags)Reader.ReadUInt32();
            Name = Reader.ReadPString();

            //The server is shortchanging us?
            //var options = (ServerOptions)Reader.ReadUInt32();
            //var ulUploadCaps = Reader.ReadUInt32();
            //var ulDownloadCaps = Reader.ReadUInt32();

            Debug.WriteLine("Name: " + Name);
            Debug.WriteLine("Permissions: " + Permissions);
        }
        public MH_ServerInfo(string Name, ServerPermissionsFlags Permissions)
        {
            this.Name = Name;
            this.Permissions = Permissions;
        }

        public ServerPermissionsFlags Permissions { get; private set; }
        public string Name { get; private set; }

        public byte[] Write(int myID)
        {
            Writer.WriteStruct(new ClientMessage
            {
                eventType = MessageTypes.SERVERINFO,
                length = sizeof(Int32) + 63 /*pstring(63)*/,
                refNum = myID
            });
            Writer.Write((uint)Permissions);
            Writer.Write(Name.ToPString(63));

            return base.Write();
        }
    }
}