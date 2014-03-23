using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Palace.Assets;
using Palace.Messages;
using Palace;
using Palace.Messages.Structures;

namespace Taj.Assets
{
    public class FlatFileManager : IAssetManager
    {
        const string _assetDirectory = "Assets";

        public FlatFileManager()
        {
            Directory.CreateDirectory(_assetDirectory);
        }

        public void PutAsset(byte[] data, AssetType type, int ID, int CRC = 0)
        {
            var prop = new PalaceProp(data, type, ID, CRC);

            File.WriteAllBytes(CreateMuddyFilename(type, ID, CRC), data);
        }

        public byte[] GetAsset(AssetType type, int ID, int CRC = 0)
        {
            return File.ReadAllBytes(CreateMuddyFilename(type, ID, CRC));
        }

        string CreateMuddyFilename(AssetType type, int ID, int CRC = 0)
        {
            return Path.Combine(_assetDirectory, string.Format("{1}{2}.{0}", type, ID, CRC));
        }
    }
}
