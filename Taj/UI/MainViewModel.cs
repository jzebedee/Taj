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
    internal class RaiseEventTraceListener : DefaultTraceListener
    {
        private readonly Action<string> toCall;
        internal RaiseEventTraceListener(Action<string> onTraceEvent)
        {
            toCall = onTraceEvent;
        }

        public override void WriteLine(string message)
        {
            toCall(message);
            base.WriteLine(message);
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private PalaceConnection _palCon;

        public MainViewModel()
        {
            PalaceConnectCommand = new ActionCommand(PalaceConnect, () => true);
        }

        private void PalaceConnect()
        {
#if TRACE
            if (!Trace.AutoFlush)
            {
                File.Delete("trace.log");
                var raiseevent_listener = new RaiseEventTraceListener((str) => Output = str);
                var file_log_listener = new TextWriterTraceListener("trace.log");
                var console_listener = new ConsoleTraceListener();
                Trace.Listeners.Add(raiseevent_listener);
                Trace.Listeners.Add(file_log_listener);
                Trace.Listeners.Add(console_listener);
                Trace.AutoFlush = true;
            }
#endif

            var identity = new PalaceUser { Name = "Superduper" };

            //var pal = new Palace(new Uri("tcp://chat.epalaces.com:9998"));
            _palCon = new PalaceConnection(new Uri("tcp://oceansapart.epalaces.com:9998"), identity);
            _palCon.Listener.ContinueWith((listenTask) => Connected = false);

            _palCon.Connect();
            Connected = true;
        }

        private bool _connected = false;
        public bool Connected
        {
            get { return _connected; }
            private set
            {
                if (_connected != value)
                {
                    _connected = value;
                    RaisePropertyChanged("Connected");
                }
            }
        }

        private StringBuilder _output = new StringBuilder();
        public string Output
        {
            get { return _output.ToString(); }
            private set
            {
                _output.AppendLine(value);
                RaisePropertyChanged("Output");
            }
        }

        private ICommand _palaceConnectCommand;
        public ICommand PalaceConnectCommand
        {
            get { return _palaceConnectCommand; }
            private set
            {
                if (_palaceConnectCommand != value)
                {
                    _palaceConnectCommand = value;
                    RaisePropertyChanged("PalaceConnectCommand");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
