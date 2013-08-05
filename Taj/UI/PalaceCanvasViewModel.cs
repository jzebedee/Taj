using Palace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Taj.UI
{
    public class PalaceCanvasViewModel : BaseNotificationModel
    {
        private IPalace _palace;
        public IPalace Palace
        {
            get
            {
                return _palace;
            }
            set
            {
                if (_palace != value)
                {
                    _palace = value;
                    RaisePropertyChanged();
                }
            }
        }

        public PalaceCanvasViewModel()
        {
        }

        public void SetBackground(Uri target)
        {
            Background = new BitmapImage(target);
        }

        private ImageSource _background;
        public ImageSource Background
        {
            get { return _background; }
            set
            {
                if (_background != value)
                {
                    _background = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}