using System.Diagnostics;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    /// <summary>
    /// These two messages are used to transmit assets from one machine to another. The 
    /// two messages are identical except for the message type. MSG_ASSETREGI is 
    /// used when the client is sending assets to the server. MSG_ASSETSEND is used 
    /// when the server is sending assets to the client.
    /// 
    /// The message format is designed to enable assets to be transmitted in blocks, with 
    /// each block sent in a separate message. In theory.
    /// 
    /// However, the Unix server only understands assets sent 
    /// whole (i.e., as a single block containing the entire asset). Thus, in practice, the 
    /// message always contains an AssetDescriptor, blockSize is always equal 
    /// to size, blockOffset is always 0, blockNbr is always 0, and nbrBlocks 
    /// is always 1.
    /// </summary>
    public class MH_AssetSend : MessageHeader
    {
        private int _ID;
        private uint _CRC;

        public MH_AssetSend(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            var assetMsg = Reader.ReadStruct<AssetSend>();

            Debug.Assert(assetMsg.blockSize == assetMsg.desc.size);
            Debug.Assert(assetMsg.blockOffset == 0);
            Debug.Assert(assetMsg.blockNbr == 0);
            Debug.Assert(assetMsg.nbrBlocks == 1);

            //blockSize is the size of the block being sent in this message.
            //data contains the actual bytes of the asset itself.
            var data = Reader.ReadBytes(assetMsg.blockSize);
            Debug.WriteLine("Prop `{0}`", new[] { assetMsg.desc.name.MarshalPString() });

            Assets.PutAsset(data, assetMsg.type, assetMsg.spec.id, assetMsg.spec.crc);
        }
        public MH_AssetSend(IPalaceConnection con, int ID, uint CRC = 0)
            : base(con)
        {
            _ID = ID;
            _CRC = CRC;
        }
    }
}