using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Taj.UI
{
    public class PalacePropViewModel : BaseViewModel
    {
        public PalacePropViewModel()
        {
        }

        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    RaisePropertyChanged("X");
                }
            }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    RaisePropertyChanged("Y");
                }
            }
        }
    }
}
