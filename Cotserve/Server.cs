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
using System.Diagnostics;

namespace Cotserve
{
    public class Server : IDisposable
    {
        const string
            SERVER_NAME = "Test",
            SERVER_VERSION = "1.23";

        private int _idCtr = 0;
        private int NewID { get { return ++_idCtr; } }

        private CancellationTokenSource _cts = null;
        private CancellationToken Token { get { return _cts.Token; } }

        private readonly Palace Palace;

        public Server()
        {
            Palace = new Palace();
        }
        ~Server()
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
                Palace.Dispose();
        }

        public async void Start()
        {
            TcpListener listener = null;
            _cts = new CancellationTokenSource();

            try
            {
                listener = new TcpListener(9998);
                listener.Start();

                while (!Token.IsCancellationRequested)
                {
                    HandleClient(await listener.AcceptTcpClientAsync());
                }
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
                Stop();
            }
        }
        public void Stop()
        {
            if (_cts == null)
                throw new InvalidOperationException("Stop called before server was started");

            _cts.Cancel();
        }

        private async Task HandleClient(TcpClient client)
        {
            using (client)
            using (var cstream = client.GetStream())
            {
                using (var Writer = new BinaryWriter(cstream))
                {
                    var myID = Handshake(Writer);

                    var buf = new byte[0x1000];
                    int bytesRead;

                    while ((bytesRead = await cstream.ReadAsync(buf, 0, ClientMessage.Size, Token)) > 0)
                    {
                        var head = buf.MarshalStruct<ClientMessage>();
                        Trace.Assert(head.length + ClientMessage.Size < 0x1000);

                        if (await cstream.ReadAsync(buf, ClientMessage.Size, head.length, Token) != head.length)
                            break;

                        var outMsg = HandleMessage(myID, head, buf.TrimBuffer(ClientMessage.Size, head.length));
                        if (outMsg != null)
                        {
                            await cstream.WriteAsync(outMsg, 0, outMsg.Length, Token);
                        }
                    }
                }
            }
        }

        private byte[] HandleMessage(int myID, ClientMessage head, byte[] backing)
        {
            switch (head.eventType)
            {
                //all new client greets happen here
                case MessageTypes.LOGON:
                    var msg_logon = new MH_Logon(head, backing);
                    var clientRec = msg_logon.Record;

                    //var newUser = new PalaceUser()
                    //{
                    //    Name = clientRec.userName.MarshalPString(),
                    //    RoomID = clientRec.desiredRoom,
                    //    ID = myID,
                    //};

                    var ALRbytes = new MH_AltLogonReply(head, backing).Write(myID, clientRec);
                    var Vbytes = new MH_ServerVersion(Version.Parse(SERVER_VERSION)).Write();
                    var SIbytes = new MH_ServerInfo(SERVER_NAME, ServerPermissionsFlags.AllowGuests | ServerPermissionsFlags.DeathPenalty).Write(myID);
                    var USbytes = new MH_UserStatus(myID, UserFlags.God).Write();
                    return Extensions.ChainAppendBuffer(ALRbytes, Vbytes, SIbytes, USbytes);
                default:
                    System.Diagnostics.Debugger.Break();
                    throw new ArgumentException("Unhandled protocol.");
            }
        }

        private int Handshake(BinaryWriter s)
        {
            var id = NewID;

            s.Write((uint)MessageTypes.TIYID);
            s.Write(0);
            s.Write(id);

            return id;
        }
    }
}