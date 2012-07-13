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
                var tuple = Handshake(stream);
                using (var reader = tuple.Item1)
                {
                    using (var writer = tuple.Item2)
                    {
                        while (connection.Connected)
                        {
                            if (stream.DataAvailable)
                            {
                                var msg = new ClientMsg(reader);
                                Console.WriteLine("{0} | {1} | {2}", msg.eventType, msg.length, msg.refNum);

                                switch (msg.eventType)
                                {
                                    case MessageTypes.Talk:
                                        Console.WriteLine("EvT: Talk");
                                        //var talk = reader.ReadStruct<ClientMsg_talk>((int)msg.length);
                                        Console.WriteLine("msg: `{0}`", reader.ReadCString());
                                        break;
                                    default:
                                        Console.WriteLine("Unknown EvT: {0}", msg.eventType);
                                        var sb = new StringBuilder();
                                        foreach (var b in reader.ReadBytes((int)msg.length))
                                            sb.Append(b);

                                        Console.WriteLine("** {0}", sb.ToString());
                                        break;
                                }
                                Console.WriteLine("--");
                            }
                        }
                        Console.WriteLine("% No more data available.");
                    }
                }
            }
            Console.WriteLine("% Listener terminated.");
        }

        Tuple<EndianBinaryReader, EndianBinaryWriter> Handshake(Stream palstream)
        {
            //We do this 'dirty' because of the extra handling for the yet-unknown endianness

            EndianBinaryReader br;
            EndianBinaryWriter bw;

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

                br = new EndianBinaryReader(endianness, palstream);
                bw = new EndianBinaryWriter(endianness, palstream);
            }

            var length = br.ReadUInt32();
            var refNum = br.ReadUInt32(); //userID for client

            byte[] msgBuffer;
            unsafe
            {
                msgBuffer = new byte[sizeof(ClientMsg_logOn)];

                var logonMsg = new ClientMsg_logOn("Superduper");
                fixed (byte* pBuf = msgBuffer)
                {
                    *((ClientMsg_logOn*)pBuf) = logonMsg;
                }
            }
            bw.Write(msgBuffer, 0, msgBuffer.Length);
            bw.Flush();

            return Tuple.Create(br, bw);
        }
    }
}
