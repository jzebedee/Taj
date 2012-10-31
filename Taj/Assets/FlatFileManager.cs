using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taj.Assets
{
    public class FlatFileManager : IAssetManager
    {
        public void PutAsset(byte[] data, Messages.AssetType type, uint ID, uint CRC)
        {
            throw new NotImplementedException();
        }

        public byte[] GetAssetByID(uint ID)
        {
            throw new NotImplementedException();
        }

        public byte[] GetAsset(uint CRC)
        {
            throw new NotImplementedException();
        }
    }
}
