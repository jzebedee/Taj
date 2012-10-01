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
    /*
    sint8 signed, 8-bit (1-byte) integer
    uint8 unsigned, 8-bit (1-byte) integer
    char 1-byte character (sign not relevant)
    sint16 signed, 16-bit (2-byte) integer
    uint16 unsigned, 16-bit (2-byte) integer
    sint32 signed, 32-bit (4-byte) integer
    uint32 unsigned, 32-bit (4-byte) integer
    */
    public class Palace : IDisposable
    {
        readonly TcpClient connection;
        readonly Task Listener;

        EndianBinaryReader reader;
        EndianBinaryWriter writer;

        public Palace(Uri target)
        {
            connection = new TcpClient(target.Host, target.Port);

            Listener = Task.Factory.StartNew(Listen, TaskCreationOptions.LongRunning);
        }
        ~Palace()
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
            byte[] buf;

            using (var stream = connection.GetStream())
            {
                Handshake(stream);
                using (reader)
                {
                    using (writer)
                    {
                        while (connection.Connected)
                        {
                            if (stream.DataAvailable)
                            {
                                var msg = new ClientMsg(reader);
#if TRACE
                                Trace.WriteLine(string.Format("{0:X8} | {1} | {2}", msg.eventType, msg.length, msg.refNum));
#endif

                                switch (msg.eventType)
                                {
                                    case MessageTypes.Talk:
#if TRACE
                                        Trace.WriteLine("EvT: Talk");
#endif
                                        //var talk = reader.ReadStruct<ClientMsg_talk>(msg.length);
#if TRACE
                                        Trace.WriteLine(string.Format("msg: `{0}`", reader.ReadCString()));
#endif
                                        break;
                                    default:
#if TRACE
                                        Trace.WriteLine("Unknown EvT");
#endif
                                        var sb = new StringBuilder();
                                        foreach (var b in reader.ReadBytes(msg.length))
                                            sb.Append(b);

#if TRACE
                                        Trace.WriteLine(string.Format("** {0}", sb.ToString()));
#endif
                                        break;
                                }
#if TRACE
                                Trace.WriteLine("--");
#endif
                            }
                        }
#if TRACE
                        Trace.WriteLine("% No more data available.");
#endif
                    }
                }
            }
#if TRACE
            Trace.WriteLine("% Listener terminated.");
#endif
        }

        public void Write(IFormattedMessage msg)
        {
            byte[] msgBuffer = msg.GetBytes();
            writer.Write(msgBuffer, 0, msgBuffer.Length);
            writer.Flush();
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

            var logonMsg = new ClientMsg_logOn("Superduper");
            Write(logonMsg);
        }
    }
}
