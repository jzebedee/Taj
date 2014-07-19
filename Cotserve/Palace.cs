using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotserve
{
    class Palace : IDisposable
    {
        readonly ConcurrentDictionary<int, PalaceRoom> _rooms = new ConcurrentDictionary<int, PalaceRoom>();

        public Palace()
        {
            _rooms.TryAdd(0, new PalaceRoom(0, "GATE"));
            _rooms.TryAdd(1, new PalaceRoom(1, "TestA"));
            _rooms.TryAdd(2, new PalaceRoom(2, "TestB"));
            _rooms.TryAdd(3, new PalaceRoom(3, "TestC"));
            _rooms.TryAdd(4, new PalaceRoom(4, "TestD"));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
