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
using Palace.Messages.Flags;
using MiscUtil.IO;

namespace Cotserve
{
    public class PalaceClient : IDisposable
    {
        private static volatile int _id = 0;

        private readonly TcpClient _client;

        protected IPalaceConnection connection { get; private set; }
        protected IPalace parent { get; private set; }

        protected readonly CancellationTokenSource _cts = new CancellationTokenSource();

        protected readonly int ID = ++_id;

        public CancellationTokenSource Source { get { return _cts; } }
        public CancellationToken Token { get { return _cts.Token; } }

        public Task LoopTask { get; private set; }

        public PalaceUser User { get; private set; }

        public PalaceClient(IPalace palace, TcpClient client)
        {
            parent = palace;
            _client = client;
            LoopTask = Task.Factory.StartNew(Loop, Token);
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
            if (disposing)
            {
                _client.Close();
                LoopTask.Wait();
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
                    {
                        connection = new PalaceClientConnection(parent, stream);
                        using (connection.Reader)
                        using (connection.Writer)
                        {
                            Handshake(connection.Writer);

                            while (_client.Connected)
                            {
                                var bufHeader = new byte[ClientMessage.Size];
                                var bytesRead = await stream.ReadAsync(bufHeader, 0, ClientMessage.Size, Token);

                                var header = bufHeader.MarshalStruct<ClientMessage>();
                                var response = HandleMessage(connection.Reader, header);

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

        private IOutgoingMessage HandleMessage(EndianBinaryReader reader, ClientMessage header)
        {
            switch (header.eventType)
            {
                case MessageTypes.LOGON:
                    var msg_logon = new MH_AltLogonReply(connection);
                    var clientRec = msg_logon.Record;

                    var newUser = new PalaceUser(parent)
                    {
                        Name = clientRec.userName.MarshalPString(),
                        RoomID = clientRec.desiredRoom,
                        ID = ID,
                    };

                    msg_logon.User = newUser;
                    User = newUser;

                    return msg_logon;
                default:
                    System.Diagnostics.Debugger.Break();
                    throw new ArgumentException("Unhandled protocol.");
            }
        }

        private void Handshake(EndianBinaryWriter writer)
        {
            writer.Write((uint)MessageTypes.TIYID);
            writer.Write(0);
            writer.Write(ID);
        }
    }
}
