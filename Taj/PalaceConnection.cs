//#define OFFLINE
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MiscUtil.Conversion;
using MiscUtil.IO;
using Taj.Assets;
using Palace.Assets;
using Palace.Messages;
using Palace.Messages.Structures;
using Palace;
using System.Collections.Generic;

namespace Taj
{
    public class PalaceConnection : IDisposable, IClientPalaceConnection
    {
        public readonly Task Listener;

        private readonly CancellationTokenSource listenerTokenSrc = new CancellationTokenSource();
        private CancellationToken listenerToken { get { return listenerTokenSrc.Token; } }

        private readonly Uri targetUri;

        public EventHandler Connected = (sender, e) => { };

        public PalaceConnection(Uri target, PalaceIdentity identity)
        {
            targetUri = target;
            Identity = identity;

            Listener = new Task(Listen, listenerToken);

            AssetStore = new FlatFileManager();
        }

        public void Connect()
        {
            Listener.Start();
        }

        public void Disconnect()
        {
            listenerTokenSrc.Cancel();
            _connection.Close();

            Listener.Wait();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region IPalaceConnection Members

        public EndianBinaryReader Reader { get; private set; }
        public EndianBinaryWriter Writer { get; private set; }

        public IPalace Palace { get; private set; }
        public PalaceIdentity Identity { get; private set; }

        public IAssetManager AssetStore { get; private set; }

        #endregion

        ~PalaceConnection()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            //Disconnect();

            listenerTokenSrc.Dispose();

            if (Reader != null)
            {
                Reader.Dispose();
                Reader = null;
            }
            if (Writer != null)
            {
                Writer.Dispose();
                Writer = null;
            }

            if (disposing)
                GC.SuppressFinalize(this);
        }

#if OFFLINE
        List<byte[]> blowthrus = new List<byte[]>();
#endif

        MH_Logon reset;
        private TcpClient _connection;
        private void Listen()
        {
#if OFFLINE
            {
                var x = @"C:\Users\James\Desktop\convo.raw";
                using (var fs = File.OpenRead(x))
                {
                    using (Reader = new EndianBinaryReader(new LittleEndianBitConverter(), fs))
                    {
                        Palace = new ClientPalace(this);

                        while (fs.Position < fs.Length)
                        {
                            var cm = Reader.ReadStruct<ClientMessage>();
                            HandleMessage(cm);
                        }


                        foreach (var bt in blowthrus)
                        {
                            using (var ws = File.OpenWrite(@"C:\Users\James\Desktop\blowthrus" + blowthrus.IndexOf(bt) + ".raw"))
                            {
                                foreach (var b in bt)
                                    ws.WriteByte(b);
                                ws.Flush();
                            }
                        }
                    }
                }
            }
#endif

            while (!listenerToken.IsCancellationRequested)
            {
                try
                {
                    _connection = new TcpClient(targetUri.Host, targetUri.Port);
                    Palace = new ClientPalace(this);

                    using (var stream = _connection.GetStream())
                    {
                        Handshake(stream, reset);
                        using (Reader)
                        using (Writer)
                        {
                            //var op_msg = new MH_SMsg("This client is running Taj DEBUG build. Please notify Scorpion of any questions or concerns.");
                            //op_msg.Write(Writer);
                            //Debug.WriteLine("OP_SMSG sent");

                            //var timer = new Timer(o => { connection.Close(); }, null, 6000, 3000);

                            Connected(this, new EventArgs());

                            while (!listenerToken.IsCancellationRequested)
                            {
                                //try
                                //{
                                var msg = Reader.ReadStruct<ClientMessage>();

                                Debug.WriteLine("Message: " + Enum.GetName(typeof(MessageTypes), msg.eventType));
                                Debug.WriteLine("{");
                                Debug.Indent();
                                HandleMessage(msg);
                                Debug.Unindent();
                                Debug.WriteLine("}");
                                //}
                                //catch (AggregateException ae)
                                //{
                                //    foreach (var ie in ae.InnerExceptions)
                                //        if (ie is ObjectDisposedException || ie is OperationCanceledException)
                                //            return;
                                //    throw;
                                //}
                            }

                            if (_connection.Connected)
                                Signoff();
                        }
                    }
                    Debug.WriteLine("Listening ended");
                }
                catch (IOException e)
                {
                    Trace.TraceError(e.ToString());
                }
                finally
                {
                    if (_connection != null)
                        _connection.Close();
                }
            }
        }

        private void HandleMessage(ClientMessage msg)
        {
            switch (msg.eventType)
            {
                case MessageTypes.ALTLOGONREPLY:
                    var altreset = new MH_Logon(this);
                    var altrec = altreset.Record;
#if OFFLINE
                    break;
#endif
                    var rec = reset.Record;
                    Debug.WriteLine("AltLogonReply.");
                    if (altrec.puidCRC != rec.puidCRC && altrec.puidCtr != rec.puidCtr)
                    {
                        Debug.WriteLine("Server changed our puid data. Reconnecting.");
                        Signoff();
                    }
                    break;
                case MessageTypes.PING:
                    Debug.WriteLine("Ping.");
                    var msg_pingpong = new MH_PingPong(this, msg);
                    msg_pingpong.Write();
                    Debug.WriteLine("Pong.");
                    break;
                case MessageTypes.USERSTATUS:
                    var msg_ustatus = new MH_UserStatus(this, msg);
                    break;
                case MessageTypes.USERMOVE:
                    var msg_umove = new MH_UserMove(this, msg);
                    break;
                case MessageTypes.USERPROP:
                    var msg_uprop = new MH_UserProp(this, msg);
                    break;
                case MessageTypes.USERLOG:
                    var msg_ulog = new MH_UserLog(this, msg);
                    break;
                case MessageTypes.USERLIST:
                    var msg_ulist = new MH_UserList(this, msg);
                    break;
                case MessageTypes.USEREXIT:
                    var msg_uexit = new MH_UserExit(this, msg);
                    break;
                case MessageTypes.USERNEW:
                    var msg_unew = new MH_UserNew(this, msg);
                    break;
                case MessageTypes.LOGOFF:
                    var msg_logoff = new MH_Logoff(this, msg);
                    break;
                case MessageTypes.TALK:
                    var msg_talk = new MH_Talk(this);
                    Debug.WriteLine("(fromuser {1}) msg: `{0}`", msg_talk.Text, msg.refNum);
                    break;
                case MessageTypes.XTALK:
                    var msg_xtalk = new MH_XTalk(this, msg);
                    Debug.WriteLine("(fromuser {1}) msg: `{0}`", msg_xtalk.Text, msg.refNum);
                    //if (Identity.ID != msg.refNum)
                    //{
                    //    var newxtalk = new MH_XTalk(this, clever.Think(msg_xtalk.Text));
                    //    newxtalk.Write();
                    //}
                    break;
                case MessageTypes.WHISPER:
                    var msg_whisp = new MH_Whisper(this, msg);
                    Debug.WriteLine("(fromuser {1}) msg: `{0}`", msg_whisp.Text, msg_whisp.Target.ID);
                    //if (msg_whisp.Target.ID != Identity.ID)
                    //{
                    //    var msg_whisp_out = new MH_Whisper(this, msg_whisp.Target, new string(msg_whisp.Text.Reverse().ToArray()));
                    //    msg_whisp_out.Write();
                    //}
                    break;
                case MessageTypes.XWHISPER:
                    var msg_xwhisp = new MH_XWhisper(this, msg);
                    Debug.WriteLine("(fromuser {1}) msg: `{0}`", msg_xwhisp.Text, msg_xwhisp.Target.ID);
                    //if (msg_xwhisp.Target.ID != Identity.ID)
                    //{
                    //    //var msg_xwhisp_out = new MH_XWhisper(this, msg_xwhisp.Target, clever.Think(msg_xwhisp.Text));
                    //    var msg_xwhisp_out = new MH_XWhisper(this, msg_xwhisp.Target, new string(msg_xwhisp.Text.Reverse().ToArray()));
                    //    msg_xwhisp_out.Write();
                    //}
                    break;
                case MessageTypes.ROOMDESC:
                    var msg_roomdesc = new MH_RoomDesc(this, msg);
                    break;
                case MessageTypes.ROOMDESCEND:
                    break;
                case MessageTypes.ASSETSEND:
                    var msg_assetsend = new MH_AssetSend(this, msg);
                    break;
                case MessageTypes.VERSION:
                    var mh_sv = new MH_ServerVersion(this, msg);
                    break;
                case MessageTypes.SERVERINFO:
                    var msg_svinfo = new MH_ServerInfo(this, msg);
                    break;
                case MessageTypes.HTTPSERVER:
                    var msg_httpsv = new MH_HTTPServer(this);
                    break;
                case MessageTypes.BLOWTHRU:
                    var msg_blowthru = new MH_Blowthru(this, msg);
#if OFFLINE
                    blowthrus.Add(msg_blowthru.Payload);
#endif
                    break;
                default:
                    Debug.WriteLine("Unknown EvT: {0} (0x{1:X8})", msg.eventType, (uint)msg.eventType);
                    Reader.ReadBytes(msg.length);
                    break;
            }
        }

        private void Signoff()
        {
            var mh_logoff = new MH_Logoff(this);
            mh_logoff.Write();
        }

        private void Handshake(Stream palstream, MH_Logon handshake = null)
        {
            //We do this 'dirty' because of the extra handling for the yet-unknown endianness

            {
                var buf = new byte[4];
                palstream.Read(buf, 0, 4);

                EndianBitConverter endianness;

                int eventType = BitConverter.ToInt32(buf, 0);
                switch ((MessageTypes)eventType)
                {
                    case MessageTypes.DIYIT:
                        endianness = EndianBitConverter.Big;
                        Debug.WriteLine("BigEndian server handshake");
                        break;
                    case MessageTypes.TIYID:
                        endianness = EndianBitConverter.Little;
                        Debug.WriteLine("LittleEndian server handshake");
                        break;
                    default:
                        throw new Exception(string.Format("unrecognized handshake event: 0x{0:X8}", eventType));
                }

                Reader = new EndianBinaryReader(endianness, palstream);
                Writer = new EndianBinaryWriter(endianness, palstream);
            }

            uint length = Reader.ReadUInt32();
            int refNum = Reader.ReadInt32(); //userID for client

            Palace.CurrentUser = Palace.GetUserByID(refNum, true);

            short desiredRoom = 0;
            short.TryParse(targetUri.AbsolutePath.TrimStart('/'), out desiredRoom);

            reset = handshake ?? new MH_Logon(this, Identity.Name, desiredRoom);
            reset.Write();
        }
    }
}