using Palace.Messages.Flags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace
{
    public interface IPalace : INotifyPropertyChanged
    {
        string Name { get; set; }
        int UserCount { get; set; }
        Version Version { get; set; }
        string HTTPServer { get; set; }
        ServerPermissionsFlags Permissions { get; set; }
        ObservableCollection<PalaceRoom> Rooms { get; }
        ObservableCollection<PalaceUser> Users { get; }

        PalaceRoom CurrentRoom { get; set; }
        PalaceUser CurrentUser { get; set; }

        PalaceRoom GetRoomByID(int RoomID, bool create = false);
        PalaceUser GetUserByID(int UserID, bool create = false);

        void RemoveRoom(PalaceRoom targetRoom);
        void RemoveRoomByID(int RoomID);
        void RemoveUser(PalaceUser targetUser);
        void RemoveUserByID(int UserID);
    }
}
