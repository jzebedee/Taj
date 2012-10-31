using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taj.Messages.Flags;

namespace Taj
{
    public class PalaceRoom : BaseNotificationModel
    {
        public Palace Host { get; protected set; }
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

        public IEnumerable<PalaceUser> Users
        {
            get
            {
                return (from u in Host.Users where u.RoomID == ID select u);
            }
        }

        public void UpdateUsers() {
            RaisePropertyChanged("Users");
        }

        public PalaceRoom(Palace host)
        {
            Host = host;
        }

        public override string ToString()
        {
            return string.Format("`{0}` ({1})", Name, ID);
        }
    }
}
