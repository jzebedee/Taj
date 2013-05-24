using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taj.Messages;

namespace Taj.Assets
{
    public interface IAssetManager
    {
        void PutAsset(byte[] data, AssetType type, uint ID, uint CRC = 0);
        byte[] GetAsset(AssetType type, uint ID, uint CRC = 0);
    }
}
