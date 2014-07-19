using System.Linq;
using Palace.Messages;
using Palace.Messages.Flags;
using Palace.Messages.Structures;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Palace
{
    public class PalaceUser : PalaceObject
    {
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

        List<AssetSpec> _specs;
        public List<AssetSpec> PropRecords
        {
            get { return _specs; }
            set
            {
                if (value != _specs)
                {
                    _specs = value;
                    RaisePropertyChanged();
                }
            }
        }

        List<PalaceProp> _props;
        public List<PalaceProp> Props
        {
            get { return _props; }
            set
            {
                if (value != _props)
                {
                    _props = value;

                    Graphics basic = null;
                    int width = 0;
                    int height = 0;
                    foreach (var prop in _props)
                    {
                        var pbm = prop.PropBitmap;
                        if (pbm == null)
                            continue;

                        if (basic == null)
                            basic = Graphics.FromImage(pbm);
                        else
                        {
                            basic.DrawImage(pbm, new System.Drawing.Point(0, 0));
                        }

                        if (width < pbm.Width)
                            width += pbm.Width;
                        if (height < pbm.Height)
                            height += pbm.Height;
                    }
                    basic.DrawRectangle(Pens.Azure, 0f, 0f, width, height);

                    if (basic != null)
                    {
                        Showing = new Bitmap(width, height, basic);
                    }

                    RaisePropertyChanged();
                }
            }
        }

        public override string ToString()
        {
            return string.Format("`{0}` ({1})", Name, ID);
        }
    }
}