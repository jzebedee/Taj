using Palace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Taj.Assets;

namespace Taj.UI
{
    public class DebugViewModel : BaseNotificationModel
    {
        private PalaceConnection _palCon;

        public DebugViewModel()
        {
#if TRACE
            File.Delete("trace.log");
            var callback_listener = new CallbackTraceListener(str => Output = str);
            var file_log_listener = new TextWriterTraceListener("trace.log");
            var console_listener = new ConsoleTraceListener();
            Trace.Listeners.Add(callback_listener);
            Trace.Listeners.Add(file_log_listener);
            Trace.Listeners.Add(console_listener);
            Trace.AutoFlush = true;
#endif

            PalaceConnectCommand = new ActionCommand(PalaceConnect, () => !Connected);
            PalaceDisconnectCommand = new ActionCommand(PalaceDisconnect, () => Connected);
            //ParsePropCommand = new ActionCommand(() => new PalaceProp(File.ReadAllBytes("Assets\\21849949602980276445.PROP"), Messages.AssetType.PROP, 2184994960, 2980276445), () => true);
        }

        private void PalaceConnect()
        {
            //var testPal = "ee.fastpalaces.com:9998/140";
            //var testPal = "oceansapart.epalaces.com:9998/112";
            //var testPal = "treasuresvalley.ssws.us:9998";
            var testPal = "chat.epalaces.com:9998/220";

            var identity = new PalaceIdentity { Name = new StringBuilder().Append("Superduper").Append((char)(new Random().Next(0, 255))).ToString() };
            _palCon = new PalaceConnection(new Uri("tcp://" + testPal), identity);

            var pcv = new PalaceCanvasView();
            var pcvm = (pcv.DataContext as PalaceCanvasViewModel);
            var palwindow = new Window() { Title = "PCanvas", Content = pcv };
            _palCon.Connected += (sender, e) =>
            {
                Connected = true;
                pcvm.Palace = _palCon.Palace;
            };
            palwindow.Show();

            _palCon.Listener.ContinueWith(listenTask =>
            {
                Connected = false;
                pcvm.Palace = null;
                MainView.UIContext.Send(x => palwindow.Close(), null);
            });

            _palCon.Connect();
        }

        private void PalaceDisconnect()
        {
            using (_palCon)
                _palCon.Disconnect();
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
                    RaisePropertyChanged();
                }
            }
        }

        private readonly StringBuilder _output = new StringBuilder();
        public string Output
        {
            get { return _output.ToString(); }
            private set
            {
                _output.AppendLine(value);
                RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        private ICommand _palaceDisconnectCommand;
        public ICommand PalaceDisconnectCommand
        {
            get { return _palaceDisconnectCommand; }
            private set
            {
                if (_palaceDisconnectCommand != value)
                {
                    _palaceDisconnectCommand = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ICommand _parsePropCommand;
        public ICommand ParsePropCommand
        {
            get { return _parsePropCommand; }
            private set
            {
                if (_parsePropCommand != value)
                {
                    _parsePropCommand = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
