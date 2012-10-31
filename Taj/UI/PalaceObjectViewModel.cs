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
    public class PalaceObjectViewModel : BaseNotificationModel
    {
        private PalaceObject _obj;
        public PalaceObject Object
        {
            get { return _obj; }
            set
            {
                if (value != _obj)
                {
                    _obj = value;
                    RaisePropertyChanged();
                }
            }
        }

        public PalaceObjectViewModel()
        {
        }
    }
}
