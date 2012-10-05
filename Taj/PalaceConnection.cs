﻿using System;
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

        public EndianBinaryReader Reader { get; private set; }
        public EndianBinaryWriter Writer { get; private set; }

        public Palace Palace { get; private set; }
        public PalaceUser Identity { get; private set; }

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
            bool rateLimiter = false;

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
                                            Debug.WriteLine(string.Format("msg: `{0}`", msg_talk.Text));
                                            break;
                                        case MessageTypes.MSG_XTALK:
                                            Debug.WriteLine("EvT: XTalk");
                                            var msg_xtalk = new MH_XTalk(this, msg);
                                            Debug.WriteLine(string.Format("msg: `{0}`", msg_xtalk.Text));
                                            break;
                                        case MessageTypes.MSG_WHISPER:
                                            Debug.WriteLine("EvT: Whisper");
                                            var msg_whisp = new MH_Whisper(this, msg);
                                            Debug.WriteLine(string.Format("msg: `{0}`", msg_whisp.Text));
                                            break;
                                        case MessageTypes.MSG_XWHISPER:
                                            Debug.WriteLine("EvT: XWhisper");
                                            var msg_xwhisp = new MH_XWhisper(this, msg);
                                            Debug.WriteLine(string.Format("msg: `{0}`", msg_xwhisp.Text));
                                            //var msg_xwhisp_out = new MH_XWhisper(msg_xwhisp.Target, new string(msg_xwhisp.Text.Reverse().ToArray()));
                                            //msg_xwhisp_out.Write();
                                            var msg_whisp_out = new MH_Whisper(this, msg_xwhisp.Target, new string(msg_xwhisp.Text.Reverse().ToArray()));
                                            msg_whisp_out.Write();
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

                                if (DateTime.Now.Second % 4 == 0)
                                {
                                    if (!rateLimiter)
                                    {
                                        var new_out_msg = new MH_Talk(this, "Hello. It is currently " + DateTime.Now.ToLongTimeString());
                                        new_out_msg.Write();
                                        //var xnew_out_msg = new MH_XTalk(this, "XHello. It is currently " + DateTime.Now.ToLongTimeString());
                                        //xnew_out_msg.Write();

                                        rateLimiter = true;
                                    }
                                }
                                else
                                {
                                    rateLimiter = false;
                                }
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

                Reader = new EndianBinaryReader(endianness, palstream);
                Writer = new EndianBinaryWriter(endianness, palstream);
            }

            var length = Reader.ReadUInt32();
            var refNum = Reader.ReadInt32(); //userID for client
            Identity.ID = refNum;

            //TODO: take out the debug room
            //oceansapart.epalaces.com:9998/124
            var logon = new MH_Logon(this, Identity.Name, 124); //112 landing, 124 jl room
            logon.Write();
        }
    }
}
