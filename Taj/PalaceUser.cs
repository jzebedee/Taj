﻿using Taj.Messages;
using Taj.Messages.Flags;

namespace Taj
{
    public class PalaceUser : PalaceObject
    {
        public Palace Host { get; protected set; }

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

        int _ID;
        public int ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    RaisePropertyChanged();
                }
            }
        }

        short _roomID;
        public short RoomID
        {
            get { return _roomID; }
            set
            {
                if (value != _roomID)
                {
                    _roomID = value;
                    Host.GetRoomByID(value).UpdateUsers();
                    RaisePropertyChanged();
                }
            }
        }

        UserFlags _flags;
        public UserFlags Flags
        {
            get { return _flags; }
            set
            {
                if (value != _flags)
                {
                    _flags = value;
                    RaisePropertyChanged();
                }
            }
        }

        public PalaceUser(Palace host)
        {
            Host = host;
        }

        public override string ToString()
        {
            return string.Format("`{0}` ({1})", Name, ID);
        }
    }
}