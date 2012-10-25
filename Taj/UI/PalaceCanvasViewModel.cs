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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Taj.UI
{
    public class PalaceCanvasViewModel : BaseViewModel
    {
        public PalaceCanvasViewModel()
        {
            Elements = new ObservableCollection<UIElement>();
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
                    RaisePropertyChanged("Background");
                }
            }
        }

        private ObservableCollection<UIElement> _elements;
        public ObservableCollection<UIElement> Elements
        {
            get { return _elements; }
            private set
            {
                if (_elements != value)
                {
                    _elements = value;
                    RaisePropertyChanged("Elements");
                }
            }
        }
    }
}