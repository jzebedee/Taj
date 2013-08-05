using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palace.Messages.Flags;
using System.Collections.Specialized;
using System.Threading;

namespace Palace
{
    public class PalaceRoom : BaseNotificationModel
    {
        public IPalace Host { get; protected set; }
        /// <summary>
        /// Visible identifier of the room on a palace
        /// </summary>
        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Numeric identifier of the room on a palace
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Flags describing attributes of the room
        /// </summary>
        public RoomFlags Flags { get; set; }
        /// <summary>
        /// One of a preset number of client-defined avatar
        /// appearances that should be displayed for the avatar
        /// when it is not showing a prop instead.
        /// </summary>
        public int FacesID { get; set; }

        private ObservableCollection<PalaceUser> _users;
        public ObservableCollection<PalaceUser> Users
        {
            get { return _users; }
            protected set
            {
                if (value != _users)
                {
                    _users = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<PalaceObject> _objects;
        public ObservableCollection<PalaceObject> Objects
        {
            get { return _objects; }
            protected set
            {
                if (value != _objects)
                {
                    _objects = value;
                    RaisePropertyChanged();
                }
            }
        }

        public PalaceRoom(IPalace host)
        {
            Host = host;

            Users = new ObservableCollection<PalaceUser>();
            Objects = new ObservableCollection<PalaceObject>();
        }

        public override string ToString()
        {
            return string.Format("`{0}` ({1})", Name, ID);
        }
    }
}
