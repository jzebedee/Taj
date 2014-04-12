using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Palace.Assets;
using Palace.Messages;
using Palace.Messages.Structures;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace Taj.Assets
{
    public class FlatFileManager : IAssetManager
    {
        const string _assetDirectory = "Assets";

        Dictionary<int, List<Task>> _assetNotifiers = new Dictionary<int, List<Task>>();

        public FlatFileManager()
        {
            Directory.CreateDirectory(_assetDirectory);
        }

        public void PutAsset(Bitmap bmap, int ID, int CRC = 0)
        {
            bmap.Save(CreateMuddyFilename(ID, CRC));

            List<Task> tasks;
            if (_assetNotifiers.TryGetValue(ID, out tasks))
            {
                foreach (var task in tasks)
                    task.Start();
                tasks.Clear();
            }
        }

        public Bitmap GetAsset(int ID, int CRC = 0)
        {
            var apath = CreateMuddyFilename(ID, CRC);
            if (!File.Exists(apath))
                return null;

            var bytes = File.ReadAllBytes(apath);
            return Palace.PalaceProp.BitmapFromBytes(bytes);
        }

        public Task<Bitmap> GetAssetAsync(int ID, int CRC = 0)
        {
            var gaTask = new Task<Bitmap>(() =>
            {
                var newA = GetAsset(ID, CRC);
                Trace.Assert(newA != null);

                return newA;
            });

            List<Task> tasks;
            if (!_assetNotifiers.TryGetValue(ID, out tasks))
                _assetNotifiers.Add(ID, new List<Task>());
            _assetNotifiers[ID].Add(gaTask);

            return gaTask;
        }

        string CreateMuddyFilename(int ID, int CRC = 0)
        {
            return Path.Combine(_assetDirectory, string.Format("{0}.prop", ID));
        }
    }
}
