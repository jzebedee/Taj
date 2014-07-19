using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace
{
    public abstract class PalaceObject : BaseNotificationModel
    {
        private double _x;
        public virtual double X
        {
            get { return _x; }
            set
            {
                if (value != _x)
                {
                    _x = value;
                    RaisePropertyChanged();
                }
            }
        }

        private double _y;
        public virtual double Y
        {
            get { return _y; }
            set
            {
                if (value != _y)
                {
                    _y = value;
                    RaisePropertyChanged();
                }
            }
        }

        private Bitmap _showing;
        public virtual Bitmap Showing
        {
            get { return _showing; }
            set
            {
                if (value != _showing)
                {
                    _showing = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
