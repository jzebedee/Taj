using Palace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;
using MiscUtil.Conversion;
using System.IO;
using Palace.Assets;

namespace Cotserve
{
    public class PalaceClientConnection : IPalaceConnection
    {
        private static readonly EndianBitConverter SERVER_ENDIANNESS = EndianBitConverter.Little;

        public PalaceClientConnection(IPalace parent, Stream connStream)
        {
            Palace = parent;
            Reader = new EndianBinaryReader(SERVER_ENDIANNESS, connStream);
            Writer = new EndianBinaryWriter(SERVER_ENDIANNESS, connStream);
        }

        public IPalace Palace { get; private set; }

        public IAssetManager AssetStore
        {
            get { throw new NotImplementedException(); }
        }

        public EndianBinaryReader Reader { get; private set; }

        public EndianBinaryWriter Writer { get; private set; }
    }
}
