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
                using(var reader = tuple.Item1)
                {
                    using (var writer = tuple.Item2)
                    {
                        var lol = new ClientMsg(reader);
                        Console.WriteLine(lol);
                    }
                }
            }
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
                if (eventType == MessageTypes.Handshake_BigEndian)
                    endianness = MiscUtil.Conversion.EndianBitConverter.Big;
                else if (eventType == MessageTypes.Handshake_LittleEndian)
                    endianness = MiscUtil.Conversion.EndianBitConverter.Little;
                else
                    throw new NotImplementedException(string.Format("unrecognized MSG_TIYID: {0}", eventType));

                br = new EndianBinaryReader(endianness, palstream);
                bw = new EndianBinaryWriter(endianness, palstream);
            }

            var length = br.ReadUInt32();
            var refNum = br.ReadUInt32(); //userID for client

            byte[] msgBuffer;
            unsafe
            {
                msgBuffer = new byte[sizeof(MSG_LOGON)];

                var logonMsg = new MSG_LOGON(0);
                fixed (byte* pBuf = msgBuffer)
                {
                    *((MSG_LOGON*)pBuf) = logonMsg;
                }
            }
            bw.Write(msgBuffer, 0, msgBuffer.Length);

            return Tuple.Create(br, bw);
        }
    }
}
