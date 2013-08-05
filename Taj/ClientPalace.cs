using Palace;
using Palace.Messages.Flags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taj
{
    public class ClientPalace : BaseNotificationModel, IPalace
    {
        protected readonly IClientPalaceConnection Connection;
        public ClientPalace(IClientPalaceConnection conn)
        {
            Connection = conn;

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
                    r.Users.CollectionChanged += (sender, e) =>
                    {
                        switch (e.Action)
                        {
                            case NotifyCollectionChangedAction.Add:
                                foreach (var pu in e.NewItems.Cast<PalaceUser>())
                                    UI.MainView.UIContext.Send(x => r.Objects.Add(pu), null);
                                break;
                            case NotifyCollectionChangedAction.Remove:
                                foreach (var pu in e.OldItems.Cast<PalaceUser>())
                                    UI.MainView.UIContext.Send(x => r.Objects.Remove(pu), null);
                                break;
                        }
                    };
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
