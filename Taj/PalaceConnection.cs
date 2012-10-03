using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using Taj.Messages;
using MiscUtil.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Taj
{
    public class PalaceConnection : IDisposable
    {
        TcpClient connection;
        Task Listener;

        EndianBinaryReader reader;
        EndianBinaryWriter writer;

        public Palace CurrentPalace { get; protected set; }
        public readonly PalaceUser Identity;

        public PalaceConnection(Uri target, PalaceUser identity)
        {
            connection = new TcpClient(target.Host, target.Port);

            Identity = identity;
            Listener = Task.Factory.StartNew(Listen, TaskCreationOptions.LongRunning);
        }
        ~PalaceConnection()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            connection.Close();

            if (disposing)
                GC.SuppressFinalize(this);
        }

        void Listen()
        {
            try
            {
                using (var stream = connection.GetStream())
                {
                    Handshake(stream);
                    using (reader)
                    {
                        using (writer)
                        {
                            CurrentPalace = new Palace();
                            while (connection.Connected)
                            {
                                if (stream.DataAvailable)
                                {
                                    var msg = reader.ReadStruct<ClientMessage>();
                                    switch (msg.eventType)
                                    {
                                        case MessageTypes.MSG_ALTLOGONREPLY:
                                            var msg_logon = new MHC_Logon(reader);
                                            Debug.WriteLine("AltLogonReply. But we're too cool to reconnect.");
                                            break;
                                        case MessageTypes.MSG_USERSTATUS:
                                            Debug.WriteLine(string.Format("EvT: UserStatus."));
                                            var msg_ustatus = new MH_UserStatus(msg, reader);
                                            break;
                                        case MessageTypes.MSG_USERLOG:
                                            Debug.WriteLine(string.Format("EvT: UserLog."));
                                            var msg_ulog = new MH_UserLog(msg, reader);
                                            break;
                                        case MessageTypes.MSG_TALK:
                                            Debug.WriteLine("EvT: Talk");
                                            var msg_talk = new MH_Talk(reader);
                                            Debug.WriteLine(string.Format("msg: `{0}`", msg_talk.Text));
                                            break;
                                        case MessageTypes.MSG_ROOMDESC:
                                            Debug.WriteLine(string.Format("EvT: RoomDesc."));
                                            var msg_roomdesc = new MH_RoomDesc(msg, reader);
                                            break;
                                        case MessageTypes.MSG_VERSION:
                                            var mh_sv = new MH_ServerVersion(msg);
                                            CurrentPalace.Version = mh_sv.Version;
                                            Debug.WriteLine(string.Format("EvT: ServerVersion. v{0}.", mh_sv.Version));
                                            break;
                                        case MessageTypes.MSG_SERVERINFO:
                                            Debug.WriteLine(string.Format("EvT: ServerInfo."));
                                            var msg_svinfo = new MH_ServerInfo(msg, reader);
                                            break;
                                        case MessageTypes.MSG_HTTPSERVER:
                                            Debug.WriteLine(string.Format("EvT: HTTPServer."));
                                            var msg_httpsv = new MH_HTTPServer(reader);
                                            Debug.WriteLine(string.Format("HTTPServer URI: {0}", msg_httpsv.Location));
                                            CurrentPalace.HTTPServer = msg_httpsv.Location;
                                            break;
                                        default:
                                            Debug.WriteLine(string.Format("Unknown EvT: 0x{0:X8}", msg.eventType));
                                            reader.Read(new byte[msg.length], 0, msg.length);
                                            break;
                                    }
                                    Debug.WriteLine("--");
                                }

                                //if (DateTime.Now.Second % 3 == 0)
                                //{
                                //    var new_out_msg = new MH_Talk("Hello. It is currently " + DateTime.Now.ToShortTimeString());
                                //    new_out_msg.Write(writer);
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

        void Handshake(Stream palstream)
        {
            //We do this 'dirty' because of the extra handling for the yet-unknown endianness

            {
                byte[] buf = new byte[4];
                palstream.Read(buf, 0, sizeof(Int32));

                MiscUtil.Conversion.EndianBitConverter endianness;

                int eventType = BitConverter.ToInt32(buf, 0);
                switch (eventType)
                {
                    case MessageTypes.MSG_DIYIT:
                        endianness = MiscUtil.Conversion.EndianBitConverter.Big;
                        break;
                    case MessageTypes.MSG_TIYID:
                        endianness = MiscUtil.Conversion.EndianBitConverter.Little;
                        break;
                    default:
                        throw new NotImplementedException(string.Format("unrecognized handshake event: 0x{0:X8}", eventType));
                        break;
                }

                reader = new EndianBinaryReader(endianness, palstream);
                writer = new EndianBinaryWriter(endianness, palstream);
            }

            var length = reader.ReadUInt32();
            var refNum = reader.ReadUInt32(); //userID for client
            Identity.ID = refNum;

            var logon = new MHC_Logon(Identity.Name);
            logon.Write(writer);
        }
    }
}
