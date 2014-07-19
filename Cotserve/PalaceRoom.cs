using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotserve
{
    public class PalaceRoom
    {
        readonly ConcurrentDictionary<int, PalaceUser> _users = new ConcurrentDictionary<int, PalaceUser>();
        readonly ConcurrentDictionary<int, RoomObject> _robs = new ConcurrentDictionary<int, RoomObject>();

        public PalaceRoom(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public int ID { get; private set; }
        public string Name { get; private set; }

        public IEnumerable<PalaceUser> Users
        {
            get
            {
                return _users.Values.AsEnumerable();
            }
        }
    }
}