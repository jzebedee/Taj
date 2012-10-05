using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public abstract class MessageHeader
    {
        protected readonly ClientMessage ClientMsg;

        protected Palace Palace { get { return Connection.Palace; } }
        protected EndianBinaryReader Reader { get { return Connection.Reader; } }
        protected EndianBinaryWriter Writer { get { return Connection.Writer; } }
        protected PalaceUser Identity { get { return Connection.Identity; } }

        private PalaceConnection Connection;

        public MessageHeader(PalaceConnection con, bool readHeader = false)
        {
            Connection = con;
            if (readHeader)
                ClientMsg = Reader.ReadStruct<ClientMessage>();
        }
        public MessageHeader(PalaceConnection con, ClientMessage cmsg)
            : this(con)
        {
            ClientMsg = cmsg;
        }
    }
}
