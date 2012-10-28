using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Taj.Messages;
using Taj.Messages.Flags;

namespace Taj
{
    public class Palace
    {
        public Version Version { get; set; }
        public Uri HTTPServer { get; set; }
        public ServerPermissionsFlags Permissions { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }

        private Dictionary<int, PalaceRoom> _rooms = new Dictionary<int, PalaceRoom>();
        public IEnumerable<PalaceRoom> Rooms { get { return _rooms.Values; } }

        private Dictionary<int, PalaceUser> _users = new Dictionary<int, PalaceUser>();
        public IEnumerable<PalaceUser> Users { get { return _users.Values; } }

        #region User collection methods
        PalaceUser GetUserByID(int UserID)
        {
            return _users[UserID];
        }
        public PalaceUser GetUserByID(int UserID, bool create = false)
        {
            PalaceUser u = null;

            try
            {
                u = GetUserByID(UserID);
            }
            catch (KeyNotFoundException)
            {
                if (create)
                {
                    u = new PalaceUser(this) { ID = UserID };
                    _users.Add(UserID, u);
                }
            }

            Debug.Assert(u != null);
            return u;
        }
        public bool RemoveUserByID(int UserID)
        {
            return _users.Remove(UserID);
        }
        public bool RemoveUser(PalaceUser targetUser)
        {
            return RemoveUserByID(targetUser.ID);
        }
        #endregion

        #region Room collection methods
        PalaceRoom GetRoomByID(int RoomID)
        {
            return _rooms[RoomID];
        }
        public PalaceRoom GetRoomByID(int RoomID, bool create = false)
        {
            PalaceRoom u = null;

            try
            {
                u = GetRoomByID(RoomID);
            }
            catch (KeyNotFoundException)
            {
                if (create)
                {
                    u = new PalaceRoom(this) { ID = RoomID };
                    _rooms.Add(RoomID, u);
                }
            }

            Debug.Assert(u != null);
            return u;
        }
        public bool RemoveRoomByID(int RoomID)
        {
            return _rooms.Remove(RoomID);
        }
        public bool RemoveRoom(PalaceRoom targetRoom)
        {
            return RemoveRoomByID(targetRoom.ID);
        }
        #endregion
    }
}