﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taj.Messages.Flags;
using System.Collections.Specialized;
using System.Threading;

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

        public PalaceRoom(Palace host)
        {
            Host = host;

            Users = new ObservableCollection<PalaceUser>();
            Objects = new ObservableCollection<PalaceObject>();

            Users.CollectionChanged += Users_CollectionChanged;
        }

        void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var pu in e.NewItems.Cast<PalaceUser>())
                        Taj.UI.MainView.UIContext.Send(x => Objects.Add(pu), null);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var pu in e.OldItems.Cast<PalaceUser>())
                        Taj.UI.MainView.UIContext.Send(x => Objects.Remove(pu), null);
                    break;
            }
        }

        public override string ToString()
        {
            return string.Format("`{0}` ({1})", Name, ID);
        }
    }
}
