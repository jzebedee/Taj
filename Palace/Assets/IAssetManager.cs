using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palace.Messages;
using Palace.Messages.Structures;
using System.Drawing;

namespace Palace.Assets
{
    public interface IAssetManager
    {
        void PutAsset(Bitmap bmap, int ID, int CRC = 0);
        Bitmap GetAsset(int ID, int CRC = 0);
        Task<Bitmap> GetAssetAsync(int ID, int CRC = 0);
    }
}
