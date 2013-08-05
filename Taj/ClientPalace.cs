using Palace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taj
{
    public class ClientPalace : IPalace
    {
        protected readonly IClientPalaceConnection Connection;
        public ClientPalace(IClientPalaceConnection conn)
        {
            Connection = conn;
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int UserCount
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Version Version
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Uri HTTPServer
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Palace.Messages.Flags.ServerPermissionsFlags Permissions
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<PalaceRoom> Rooms
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.ObjectModel.ObservableCollection<PalaceUser> Users
        {
            get { throw new NotImplementedException(); }
        }

        public PalaceRoom CurrentRoom
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public PalaceUser CurrentUser
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public PalaceRoom GetRoomByID(int RoomID, bool create = false)
        {
            throw new NotImplementedException();
        }

        public PalaceUser GetUserByID(int UserID, bool create = false)
        {
            throw new NotImplementedException();
        }

        public void RemoveRoom(PalaceRoom targetRoom)
        {
            throw new NotImplementedException();
        }

        public void RemoveRoomByID(int RoomID)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(PalaceUser targetUser)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserByID(int UserID)
        {
            throw new NotImplementedException();
        }
    }
}
