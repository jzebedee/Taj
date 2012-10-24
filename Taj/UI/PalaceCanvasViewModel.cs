using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Taj.UI
{
    public class PalaceCanvasViewModel : BaseViewModel
    {
        public PalaceCanvasViewModel()
        {
            Elements = new ObservableCollection<UIElement>();
            var rand = new Random();
            for (int i = 0; i < 40; i++)
            {
                var ppv = new PalacePropView();
                var ppvm = ppv.DataContext as PalacePropViewModel;
                ppvm.X = rand.NextDouble() * 500;
                ppvm.Y = rand.NextDouble() * 500;
                Elements.Add(ppv);
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