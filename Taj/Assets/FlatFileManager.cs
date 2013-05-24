using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Taj.Assets
{
    public class FlatFileManager : IAssetManager
    {
        const string _assetDirectory = "Assets";

        public FlatFileManager()
        {
            Directory.CreateDirectory(_assetDirectory);
        }

        public void PutAsset(byte[] data, Messages.AssetType type, uint ID, uint CRC = 0)
        {
            var prop = new PalaceProp(data, type, ID, CRC);

            File.WriteAllBytes(CreateMuddyFilename(type, ID, CRC), data);
        }

        public byte[] GetAsset(Messages.AssetType type, uint ID, uint CRC = 0)
        {
            return File.ReadAllBytes(CreateMuddyFilename(type, ID, CRC));
        }

        string CreateMuddyFilename(Messages.AssetType type, uint ID, uint CRC = 0)
        {
            return Path.Combine(_assetDirectory, string.Format("{1}{2}.{0}", type, ID, CRC));
        }
    }
}
