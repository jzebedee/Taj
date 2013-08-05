using MiscUtil.IO;
using Palace.Assets;
using Palace.Messages.Structures;

namespace Palace.Messages
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

        protected IPalace Palace
        {
            get { return Connection.Palace; }
        }

        protected IAssetManager Assets
        {
            get { return Connection.AssetStore; }
        }

        protected EndianBinaryReader Reader
        {
            get { return Connection.Reader; }
        }

        protected EndianBinaryWriter Writer
        {
            get { return Connection.Writer; }
        }

        protected PalaceUser CurrentUser
        {
            get { return Connection.Palace.CurrentUser; }
        }
    }
}