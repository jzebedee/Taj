using System.Diagnostics;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    /// <summary>
    /// This requests the receiver to send the sender a particular asset. The server uses it 
    /// to request props from the client, and the client uses it to request arbitrary assets.
    /// </summary>
    public class MH_AssetQuery : MessageWriter
    {
        public byte[] Write(int myID, int specID, int specCRC)
        {
            Writer.WriteStruct(new ClientMessage
            {
                eventType = MessageTypes.ASSETQUERY,
                length = AssetQuery.Size,
                refNum = myID,
            });
            Writer.WriteStruct(new AssetQuery
            {
                type = AssetType.PROP,
                spec = new AssetSpec
                {
                    id = specID,
                    crc = specCRC,
                }
            });
            Writer.Flush();

            return base.Write();
        }
    }
}