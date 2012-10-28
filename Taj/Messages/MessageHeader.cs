using MiscUtil.IO;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    public abstract class MessageHeader : IClientMessage
    {
        protected readonly ClientMessage ClientMsg;
        private readonly IPalaceConnection Connection;

        public MessageHeader(IPalaceConnection con)
        {
            Connection = con;
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