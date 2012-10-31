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
        void PutAsset(byte[] data, AssetType type, uint ID, uint CRC);
        byte[] GetAssetByID(uint ID);
        byte[] GetAsset(uint CRC);
    }
}
