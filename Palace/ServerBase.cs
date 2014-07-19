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
        private Task _listenTask;
        private CancellationTokenSource _cts;
        private readonly TcpListener _listener;

        protected ClientFactoryDelegate clientFactory { get; set; }

        public readonly EventHandler<T> ClientConnected = (sender, e) => { };
        public readonly EventHandler<T> ClientDisconnected = (sender, e) => { };

        public delegate T ClientFactoryDelegate(TcpClient netClient, EventHandler<T> clientDisconnectedCallback);

        public IList<T> Clients { get; private set; }

        protected ServerBase(IPEndPoint bind)
        {
            _listener = new TcpListener(bind);
            Clients = new List<T>();

            ClientDisconnected += (sender, e) =>
            {
                if (sender == this)
                    Clients.Remove(e);
            };
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
            if (clientFactory == null)
                throw new ArgumentNullException("ClientFactory");

            _listener.Start();

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
                    var newClient = clientFactory(_listener.AcceptTcpClient(), ClientDisconnected);
                    Clients.Add(newClient);

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