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
#if TRACE
using System.Diagnostics;
#endif

namespace Taj
{
    public class PalaceConnection : IDisposable
    {
        TcpClient connection;
        Task Listener;

        EndianBinaryReader reader;
        EndianBinaryWriter writer;

        public Palace CurrentPalace { get; set; }

        public PalaceConnection(Uri target)
        {
            connection = new TcpClient(target.Host, target.Port);

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
                                        case MessageTypes.AlternateLogonReply:
                                            var msg_logon = new MHC_Logon(reader);
                                            Console.WriteLine(msg_logon);
                                            break;
#if TRACE
                                        case MessageTypes.Talk:
                                            Trace.WriteLine("EvT: Talk");
                                            var msg_talk = new MH_Talk(reader);
                                            Trace.WriteLine(string.Format("msg: `{0}`", msg_talk.Text));
                                            break;
#endif
                                        case MessageTypes.ServerVersion:
                                            var mh_sv = new MH_ServerVersion(msg);
                                            CurrentPalace.Version = mh_sv.Version;
#if TRACE
                                            Trace.WriteLine(string.Format("EvT: ServerVersion. v{0}.", mh_sv.Version));
#endif
                                            break;
#if TRACE
                                        default:
                                            Trace.WriteLine(string.Format("Unknown EvT: {0}", msg.eventType));
                                            reader.Read(new byte[msg.length], 0, msg.length);
                                            break;
#endif
                                    }
#if TRACE
                                    Trace.WriteLine("--");
#endif
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
                    case MessageTypes.Handshake_BigEndian:
                        endianness = MiscUtil.Conversion.EndianBitConverter.Big;
                        break;
                    case MessageTypes.Handshake_LittleEndian:
                        endianness = MiscUtil.Conversion.EndianBitConverter.Little;
                        break;
                    default:
                        throw new NotImplementedException(string.Format("unrecognized MSG_TIYID: {0}", eventType));
                        break;
                }

                reader = new EndianBinaryReader(endianness, palstream);
                writer = new EndianBinaryWriter(endianness, palstream);
            }

            var length = reader.ReadUInt32();
            var refNum = reader.ReadUInt32(); //userID for client

            var logon = new MHC_Logon("Superduper");
            logon.Write(writer);
        }
    }
}
