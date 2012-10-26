using System;
using System.Collections.Generic;
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
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
#if DEBUG
            new Window() { Title = "Debug Window", Content = new DebugView() }.Show();
#endif
        }
    }
}
