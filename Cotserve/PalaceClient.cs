using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Palace.Messages;
using Palace.Messages.Structures;
using Palace;

namespace Cotserve
{
    public class PalaceClient : IDisposable
    {
        private static volatile int _id = 0;

        protected TcpClient _client { get; private set; }

        protected Task _loop { get; private set; }
        protected readonly CancellationTokenSource _cts = new CancellationTokenSource();

        protected readonly int ID = ++_id;

        public CancellationTokenSource Source { get { return _cts; } }
        public CancellationToken Token { get { return _cts.Token; } }

        public PalaceClient(TcpClient client)
        {
            _client = client;
            _loop = Task.Factory.StartNew(Loop, Token);
        }
        ~PalaceClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_client != null)
            {
                if (disposing)
                    _client.Close();
                _client = null;
            }

            if (_loop != null)
            {
                if (disposing)
                    _loop.Wait();
                _loop = null;
            }
        }

        private async void Loop()
        {
            try
            {
                using (_client)
                {
                    Console.WriteLine("Client " + ID + " connected.");

                    using (var stream = _client.GetStream())
                    using (var reader = new BinaryReader(stream))
                    {
                        using (var writer = new BinaryWriter(stream))
                        {
                            Handshake(writer);

                            while (_client.Connected)
                            {
                                var bufHeader = new byte[ClientMessage.Size];
                                var bytesRead = await stream.ReadAsync(bufHeader, 0, ClientMessage.Size, Token);

                                var header = bufHeader.MarshalStruct<ClientMessage>();
                                var response = HandleMessage(reader, header);

                                response.Write();
                            }
                        }
                    }
                }
            }
            finally
            {
                Console.WriteLine("Client " + ID + " disconnected.");
            }
        }

        private IOutgoingMessage HandleMessage(BinaryReader reader, ClientMessage header)
        {
            switch (header.eventType)
            {
                case MessageTypes.LOGON:
                    var clientRec = reader.ReadBytes(header.length).MarshalStruct<AuxRegistrationRec>();
                    Console.WriteLine(clientRec);

                    return null;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void Handshake(BinaryWriter writer)
        {
            writer.Write((uint)MessageTypes.TIYID);
            writer.Write(0);
            writer.Write(_id);
        }
    }
}
