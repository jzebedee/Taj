using Palace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Palace
{
    public abstract class ServerBase<T> : BaseNotificationModel, IDisposable
    {
        const int MAX_PENDING_CLIENTS = -1;

        private Task _listenTask;
        private Func<TcpClient, T> _clientFactory;
        private CancellationTokenSource _cts;
        private readonly TcpListener _listener;

        public readonly EventHandler<T> ClientConnected = (sender, e) => { };

        public IList<T> Clients { get; private set; }

        protected ServerBase(IPEndPoint bind, Func<TcpClient, T> clientFactory)
        {
            _listener = new TcpListener(bind);
            Clients = new List<T>();

            _clientFactory = clientFactory;
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _listener.Stop();
            }
        }
        #endregion

        public void StartServer()
        {
            if (MAX_PENDING_CLIENTS < 0)
                _listener.Start();
            else
                _listener.Start(MAX_PENDING_CLIENTS);

            _cts = new CancellationTokenSource();
            _listenTask = new Task(Listen, _cts.Token);
            _listenTask.Start();
        }

        private void Listen()
        {
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    var newClient = _clientFactory(_listener.AcceptTcpClient());
                    Clients.Add(newClient);

                    RaisePropertyChanged("Clients");

                    ClientConnected(this, newClient);
                }
                catch (SocketException e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }

        public void StopServer(int timeout = Timeout.Infinite)
        {
            _cts.Cancel();
            _listener.Stop();

            _listenTask.Wait(timeout);
        }
    }
}