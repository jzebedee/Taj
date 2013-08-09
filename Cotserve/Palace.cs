using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.IO;
using System.Net.Sockets;
using Palace;
using Palace.Messages.Flags;

namespace Cotserve
{
    public class Palace : ServerBase<PalaceClient>, IPalace
    {
        public Palace(IPEndPoint bind)
            : base(bind)
        {
            clientFactory = CreatePalaceClient;
        }

        private PalaceClient CreatePalaceClient(TcpClient netClient, EventHandler<PalaceClient> clientDisconnectedCallback)
        {
            var pclient = new PalaceClient(this, netClient);
            pclient.LoopTask.ContinueWith(t => clientDisconnectedCallback(this, pclient));

            return pclient;
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

        public ServerPermissionsFlags Permissions
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

        public ObservableCollection<PalaceRoom> Rooms
        {
            get { throw new NotImplementedException(); }
        }

        public ObservableCollection<PalaceUser> Users
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