using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MiscUtil.Conversion;
using MiscUtil.IO;
using Taj.Messages;

namespace Taj
{
    public class PalaceConnection : IDisposable, IPalaceConnection
    {
        private readonly TcpClient connection;
        public readonly Task Listener;

        public PalaceConnection(Uri target, PalaceUser identity)
        {
            connection = new TcpClient(target.Host, target.Port);

            Identity = identity;
            Listener = new Task(Listen, TaskCreationOptions.LongRunning);
        }

        public void Connect()
        {
            Listener.Start();
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

        public Palace Palace { get; private set; }
        public PalaceUser Identity { get; private set; }

        #endregion

        ~PalaceConnection()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            connection.Close();

            if (disposing)
                GC.SuppressFinalize(this);
        }

        private void Listen()
        {
            try
            {
                using (var stream = connection.GetStream())
                {
                    Handshake(stream);
                    using (Reader)
                    {
                        using (Writer)
                        {
                            Palace = new Palace();

                            //var op_msg = new MH_SMsg("This client is running Taj DEBUG build. Please notify Scorpion of any questions or concerns.");
                            //op_msg.Write(Writer);
                            //Debug.WriteLine("OP_SMSG sent");

                            //var timer = new Timer(o => { connection.Close(); }, null, 6000, 3000);
                            while (connection.Connected)
                            {
                                if (stream.DataAvailable)
                                {
                                    var msg = Reader.ReadStruct<ClientMessage>();
                                    switch (msg.eventType)
                                    {
                                        case MessageTypes.ALTLOGONREPLY:
                                            var msg_logon = new MH_Logon(this);
                                            Debug.WriteLine("AltLogonReply. But we're too cool to reconnect.");
                                            break;
                                        case MessageTypes.PING:
                                            Debug.WriteLine("Ping.");
                                            var msg_pingpong = new MH_PingPong(this, msg);
                                            msg_pingpong.Write();
                                            Debug.WriteLine("Pong.");
                                            break;
                                        case MessageTypes.USERSTATUS:
                                            Debug.WriteLine("EvT: UserStatus.");
                                            var msg_ustatus = new MH_UserStatus(this, msg);
                                            Debug.WriteLine("Target: {0}", msg_ustatus.Target);
                                            Debug.WriteLine("Flags: {0}", msg_ustatus.Target.Flags);
                                            break;
                                        case MessageTypes.USERLOG:
                                            Debug.WriteLine("EvT: UserLog.");
                                            var msg_ulog = new MH_UserLog(this, msg);
                                            break;
                                        case MessageTypes.USERLIST:
                                            Debug.WriteLine("EvT: UserList.");
                                            var msg_ulist = new MH_UserList(this, msg);
                                            break;
                                        case MessageTypes.USERNEW:
                                            Debug.WriteLine("EvT: UserNew.");
                                            var msg_unew = new MH_UserNew(this, msg);
                                            break;
                                        case MessageTypes.TALK:
                                            Debug.WriteLine("EvT: Talk");
                                            var msg_talk = new MH_Talk(this);
                                            Debug.WriteLine("(fromuser {1}) msg: `{0}`", msg_talk.Text, msg.refNum);
                                            break;
                                        case MessageTypes.XTALK:
                                            Debug.WriteLine("EvT: XTalk");
                                            var msg_xtalk = new MH_XTalk(this, msg);
                                            Debug.WriteLine(string.Format("(fromuser {1}) msg: `{0}`", msg_xtalk.Text, msg.refNum));
                                            break;
                                        case MessageTypes.WHISPER:
                                            Debug.WriteLine("EvT: Whisper");
                                            var msg_whisp = new MH_Whisper(this, msg);
                                            Debug.WriteLine("(fromuser {1}) msg: `{0}`", msg_whisp.Text, msg_whisp.Target.ID);
                                            if (msg_whisp.Target.ID != Identity.ID)
                                            {
                                                var msg_whisp_out = new MH_Whisper(this, msg_whisp.Target, new string(msg_whisp.Text.Reverse().ToArray()));
                                                msg_whisp_out.Write();
                                            }
                                            break;
                                        case MessageTypes.XWHISPER:
                                            Debug.WriteLine("EvT: XWhisper");
                                            var msg_xwhisp = new MH_XWhisper(this, msg);
                                            Debug.WriteLine("(fromuser {1}) msg: `{0}`", msg_xwhisp.Text, msg_xwhisp.Target.ID);
                                            if (msg_xwhisp.Target.ID != Identity.ID)
                                            {
                                                var msg_xwhisp_out = new MH_XWhisper(this, msg_xwhisp.Target, new string(msg_xwhisp.Text.Reverse().ToArray()));
                                                msg_xwhisp_out.Write();
                                            }
                                            break;
                                        case MessageTypes.ROOMDESC:
                                            Debug.WriteLine("EvT: RoomDesc.");
                                            var msg_roomdesc = new MH_RoomDesc(this, msg);
                                            break;
                                        case MessageTypes.ROOMDESCEND:
                                            Debug.WriteLine("EvT: RoomDescEnd.");
                                            break;
                                        case MessageTypes.VERSION:
                                            var mh_sv = new MH_ServerVersion(this, msg);
                                            Palace.Version = mh_sv.Version;
                                            Debug.WriteLine("EvT: ServerVersion. v{0}.", mh_sv.Version);
                                            break;
                                        case MessageTypes.SERVERINFO:
                                            Debug.WriteLine("EvT: ServerInfo.");
                                            var msg_svinfo = new MH_ServerInfo(this, msg);
                                            Debug.WriteLine("Name: {0}", new[] { Palace.Name });
                                            Debug.WriteLine("Permissions: {0}", Palace.Permissions);
                                            break;
                                        case MessageTypes.HTTPSERVER:
                                            Debug.WriteLine("EvT: HTTPServer.");
                                            var msg_httpsv = new MH_HTTPServer(this);
                                            Debug.WriteLine("HTTPServer URI: {0}", msg_httpsv.Location);
                                            Palace.HTTPServer = msg_httpsv.Location;
                                            break;
                                        default:
                                            Debug.WriteLine("Unknown EvT: {0} (0x{1:X8})", msg.eventType, (uint)msg.eventType);
                                            Reader.Read(new byte[msg.length], 0, msg.length);
                                            break;
                                    }
                                    Debug.WriteLine("--");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e is IOException || e is ObjectDisposedException)
                    Trace.TraceError(e.ToString());
                else
                    throw e;
            }
        }

        private void Handshake(Stream palstream)
        {
            //We do this 'dirty' because of the extra handling for the yet-unknown endianness

            {
                var buf = new byte[4];
                palstream.Read(buf, 0, sizeof(Int32));

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
                        throw new NotImplementedException(string.Format("unrecognized handshake event: 0x{0:X8}", eventType));
                }

                Reader = new EndianBinaryReader(endianness, palstream);
                Writer = new EndianBinaryWriter(endianness, palstream);
            }

            uint length = Reader.ReadUInt32();
            int refNum = Reader.ReadInt32(); //userID for client
            Identity.ID = refNum;

            //TODO: take out the debug room
            //oceansapart.epalaces.com:9998/124
            var logon = new MH_Logon(this, Identity.Name, 124); //112 landing, 124 jl room
            logon.Write();
        }
    }
}