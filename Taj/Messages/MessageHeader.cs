﻿using MiscUtil.IO;

namespace Taj.Messages
{
    public abstract class MessageHeader
    {
        protected readonly ClientMessage ClientMsg;
        private readonly IPalaceConnection Connection;

        public MessageHeader(IPalaceConnection con, bool readHeader = false)
        {
            Connection = con;
            if (readHeader)
                ClientMsg = Reader.ReadStruct<ClientMessage>();
        }

        public MessageHeader(IPalaceConnection con, ClientMessage cmsg)
            : this(con)
        {
            ClientMsg = cmsg;
        }

        protected Palace Palace
        {
            get { return Connection.Palace; }
        }

        protected EndianBinaryReader Reader
        {
            get { return Connection.Reader; }
        }

        protected EndianBinaryWriter Writer
        {
            get { return Connection.Writer; }
        }

        protected PalaceUser Identity
        {
            get { return Connection.Identity; }
        }
    }
}