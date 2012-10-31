using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taj.Messages.Structures
{
    public static class StructExtensions
    {
        public static void Populate(this UserRec rec, PalaceUser user)
        {
            user.Name = rec.name.MarshalPString();
            user.RoomID = rec.roomID;
            user.X = rec.roomPos.x;
            user.Y = rec.roomPos.y;
        }
    }
}
