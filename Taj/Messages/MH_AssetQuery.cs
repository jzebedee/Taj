using System.Diagnostics;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    /// <summary>
    /// This requests the receiver to send the sender a particular asset. The server uses it 
    /// to request props from the client, and the client uses it to request arbitrary assets.
    /// </summary>
    public class MH_AssetQuery : MessageHeader, IOutgoingMessage
    {
        private int _ID;
        private uint _CRC;

        public MH_AssetQuery(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {

        }
        public MH_AssetQuery(IPalaceConnection con, int ID, uint CRC = 0)
            : base(con)
        {
            _ID = ID;
            _CRC = CRC;
        }

        public void Write()
        {
            var payload = new AssetQuery
            {
                type = AssetType.PROP,
                spec = new AssetSpec
                {
                    id = _ID,
                    crc = _CRC,
                }
            };

            Writer.WriteStruct(new ClientMessage
            {
                eventType = MessageTypes.ASSETQUERY,
                length = AssetQuery.Size,
                refNum = CurrentUser.ID,
            });
            Writer.WriteStruct(payload);
            Writer.Flush();
        }
    }
}