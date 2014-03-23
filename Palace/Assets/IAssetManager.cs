using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palace.Messages;
using Palace.Messages.Structures;

namespace Palace.Assets
{
    public interface IAssetManager
    {
        void PutAsset(byte[] data, AssetType type, int ID, int CRC = 0);
        byte[] GetAsset(AssetType type, int ID, int CRC = 0);
    }
}
