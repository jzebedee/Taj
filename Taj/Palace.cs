using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Taj.Messages;
using Taj.Messages.Flags;

namespace Taj
{
    public class Palace : BaseNotificationModel
    {
        private IPalaceConnection _conn;
        public Palace(IPalaceConnection connection)
        {
            _conn = connection;
            Users = new ObservableCollection<PalaceUser>();
            Rooms = new ObservableCollection<PalaceRoom>();
        }

        private Version _version;
        public Version Version
        {
            get
            {
                return _version;
            }
            set
            {
                if (_version != value)
                {
                    _version = value;
                    RaisePropertyChanged();
                }
            }
        }

        private Uri _httpServer;
        public Uri HTTPServer
        {
            get
            {
                return _httpServer;
            }
            set
            {
                if (_httpServer != value)
                {
                    _httpServer = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ServerPermissionsFlags _permissions;
        public ServerPermissionsFlags Permissions
        {
            get
            {
                return _permissions;
            }
            set
            {
                if (_permissions != value)
                {
                    _permissions = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }

        private PalaceUser _curUser;
        public PalaceUser CurrentUser
        {
            get
            {
                return _curUser;
            }
            set
            {
                if (_curUser != value)
                {
                    _curUser = value;
                    RaisePropertyChanged();
                }
            }
        }

        private PalaceRoom _curRoom;
        public PalaceRoom CurrentRoom
        {
            get
            {
                return _curRoom;
            }
            set
            {
                if (_curRoom != value)
                {
                    _curRoom = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int UserCount { get; set; }

        public ObservableCollection<PalaceRoom> Rooms { get; protected set; }
        public ObservableCollection<PalaceUser> Users { get; protected set; }

        #region User collection methods
        PalaceUser GetUserByID(int UserID)
        {
            return (from u in Users where u.ID == UserID select u).Single();
        }
        public PalaceUser GetUserByID(int UserID, bool create = false)
        {
            PalaceUser u = null;

            try
            {
                u = GetUserByID(UserID);
            }
            catch (InvalidOperationException)
            {
                if (create)
                {
                    u = new PalaceUser(this) { ID = UserID };
                    Users.Add(u);
                }
            }

            Debug.Assert(u != null);
            return u;
        }
        public void RemoveUserByID(int UserID)
        {
            Debug.Assert(Users.Count(u => u.ID == UserID) <= 1);

            var found = Users.SingleOrDefault(u => u.ID == UserID);
            if (found == null) return;
            found.RoomID = default(short);

            Users.Remove(found);
        }
        public void RemoveUser(PalaceUser targetUser)
        {
            Users.Remove(targetUser);
        }
        #endregion

        #region Room collection methods
        PalaceRoom GetRoomByID(int RoomID)
        {
            return (from r in Rooms where r.ID == RoomID select r).Single();
        }
        public PalaceRoom GetRoomByID(int RoomID, bool create = false)
        {
            PalaceRoom r = null;

            try
            {
                r = GetRoomByID(RoomID);
            }
            catch (InvalidOperationException)
            {
                if (create)
                {
                    r = new PalaceRoom(this) { ID = RoomID };
                    Rooms.Add(r);
                }
            }

            Debug.Assert(r != null);
            return r;
        }
        public void RemoveRoomByID(int RoomID)
        {
            Debug.Assert(Rooms.Count(r => r.ID == RoomID) <= 1);

            var found = Rooms.SingleOrDefault(r => r.ID == RoomID);
            if (found == null) return;

            Rooms.Remove(found);
        }
        public void RemoveRoom(PalaceRoom targetRoom)
        {
            Rooms.Remove(targetRoom);
        }
        #endregion
    }
}