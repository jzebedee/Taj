using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using MiscUtil.Conversion;
using MiscUtil.IO;
using Taj.Messages;

namespace Taj
{
    public class PalaceConnection : IDisposable, IPalaceConnection
    {
        private readonly TcpClient connection;
        private Task Listener;

        public PalaceConnection(Uri target, PalaceUser identity)
        {
            connection = new TcpClient(target.Host, target.Port);

            Identity = identity;
            Listener = Task.Factory.StartNew(Listen, TaskCreationOptions.LongRunning);
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
                using (NetworkStream stream = connection.GetStream())
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

                            while (connection.Connected)
                            {
                                if (stream.DataAvailable)
                                {
                                    var msg = Reader.ReadStruct<ClientMessage>();
                                    switch (msg.eventType)
                                    {
                                        case MessageTypes.MSG_ALTLOGONREPLY:
                                            var msg_logon = new MH_Logon(this);
                                            Debug.WriteLine("AltLogonReply. But we're too cool to reconnect.");
                                            break;
                                        case MessageTypes.MSG_PING:
                                            Debug.WriteLine("Ping.");
                                            var msg_pingpong = new MH_PingPong(this, msg);
                                            msg_pingpong.Write();
                                            Debug.WriteLine("Pong.");
                                            break;
                                        case MessageTypes.MSG_USERSTATUS:
                                            Debug.WriteLine(string.Format("EvT: UserStatus."));
                                            var msg_ustatus = new MH_UserStatus(this, msg);
                                            break;
                                        case MessageTypes.MSG_USERLOG:
                                            Debug.WriteLine(string.Format("EvT: UserLog."));
                                            var msg_ulog = new MH_UserLog(this, msg);
                                            break;
                                        case MessageTypes.MSG_USERLIST:
                                            Debug.WriteLine(string.Format("EvT: UserList."));
                                            var msg_ulist = new MH_UserList(this, msg);
                                            break;
                                        case MessageTypes.MSG_USERNEW:
                                            Debug.WriteLine(string.Format("EvT: UserNew."));
                                            var msg_unew = new MH_UserNew(this, msg);
                                            break;
                                        case MessageTypes.MSG_TALK:
                                            Debug.WriteLine("EvT: Talk");
                                            var msg_talk = new MH_Talk(this);
                                            Debug.WriteLine(string.Format("(fromuser {1}) msg: `{0}`", msg_talk.Text,
                                                                          msg.refNum));
                                            break;
                                        case MessageTypes.MSG_XTALK:
                                            Debug.WriteLine("EvT: XTalk");
                                            var msg_xtalk = new MH_XTalk(this, msg);
                                            Debug.WriteLine(string.Format("(fromuser {1}) msg: `{0}`", msg_xtalk.Text,
                                                                          msg.refNum));
                                            break;
                                        case MessageTypes.MSG_WHISPER:
                                            Debug.WriteLine("EvT: Whisper");
                                            var msg_whisp = new MH_Whisper(this, msg);
                                            Debug.WriteLine(string.Format("(fromuser {1}) msg: `{0}`", msg_whisp.Text,
                                                                          msg_whisp.Target.ID));
                                            if (msg_whisp.Target.ID != Identity.ID)
                                            {
                                                var msg_whisp_out = new MH_Whisper(this, msg_whisp.Target,
                                                                                   new string(
                                                                                       msg_whisp.Text.Reverse().ToArray()));
                                                msg_whisp_out.Write();
                                            }
                                            break;
                                        case MessageTypes.MSG_XWHISPER:
                                            Debug.WriteLine("EvT: XWhisper");
                                            var msg_xwhisp = new MH_XWhisper(this, msg);
                                            Debug.WriteLine(string.Format("(fromuser {1}) msg: `{0}`", msg_xwhisp.Text,
                                                                          msg_xwhisp.Target.ID));
                                            if (msg_xwhisp.Target.ID != Identity.ID)
                                            {
                                                var msg_xwhisp_out = new MH_XWhisper(this, msg_xwhisp.Target,
                                                                                     new string(
                                                                                         msg_xwhisp.Text.Reverse().
                                                                                             ToArray()));
                                                msg_xwhisp_out.Write();
                                            }
                                            break;
                                        case MessageTypes.MSG_ROOMDESC:
                                            Debug.WriteLine(string.Format("EvT: RoomDesc."));
                                            var msg_roomdesc = new MH_RoomDesc(this, msg);
                                            break;
                                        case MessageTypes.MSG_ROOMDESCEND:
                                            Debug.WriteLine(string.Format("EvT: RoomDescEnd."));
                                            break;
                                        case MessageTypes.MSG_VERSION:
                                            var mh_sv = new MH_ServerVersion(this, msg);
                                            Palace.Version = mh_sv.Version;
                                            Debug.WriteLine(string.Format("EvT: ServerVersion. v{0}.", mh_sv.Version));
                                            break;
                                        case MessageTypes.MSG_SERVERINFO:
                                            Debug.WriteLine(string.Format("EvT: ServerInfo."));
                                            var msg_svinfo = new MH_ServerInfo(this, msg);
                                            break;
                                        case MessageTypes.MSG_HTTPSERVER:
                                            Debug.WriteLine(string.Format("EvT: HTTPServer."));
                                            var msg_httpsv = new MH_HTTPServer(this);
                                            Debug.WriteLine(string.Format("HTTPServer URI: {0}", msg_httpsv.Location));
                                            Palace.HTTPServer = msg_httpsv.Location;
                                            break;
                                        default:
                                            Debug.WriteLine(string.Format("Unknown EvT: 0x{0:X8}", msg.eventType));
                                            Reader.Read(new byte[msg.length], 0, msg.length);
                                            break;
                                    }
                                    Debug.WriteLine("--");
                                }

                                //if (DateTime.Now.Second % 4 == 0)
                                //{
                                //    if (!rateLimiter)
                                //    {
                                //        var new_out_msg = new MH_Talk(this,
                                //                                      "Hello. It is currently " +
                                //                                      DateTime.Now.ToLongTimeString());
                                //        new_out_msg.Write();
                                //        var xnew_out_msg = new MH_XTalk(this,
                                //                                        "XHello. It is currently " +
                                //                                        DateTime.Now.ToLongDateString());
                                //        xnew_out_msg.Write();

                                //        rateLimiter = true;
                                //    }
                                //}
                                //else
                                //{
                                //    rateLimiter = false;
                                //}
                            }
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Trace.TraceError(e.ToString());
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
                switch (eventType)
                {
                    case MessageTypes.MSG_DIYIT:
                        endianness = EndianBitConverter.Big;
                        Debug.WriteLine("BigEndian server handshake");
                        break;
                    case MessageTypes.MSG_TIYID:
                        endianness = EndianBitConverter.Little;
                        Debug.WriteLine("LittleEndian server handshake");
                        break;
                    default:
                        throw new NotImplementedException(string.Format("unrecognized handshake event: 0x{0:X8}",
                                                                        eventType));
                        break;
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