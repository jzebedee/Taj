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
                using (var reader = new BinaryReader(stream))
                {
                    Handshake(reader);
                }
            }
        }

        void Handshake(BinaryReader reader)
        {
            var msg = new ClientMsg(reader);
            Console.WriteLine(msg);

            /*Server smsg = (Server)Enum.Parse(typeof(Server), msgid.ToString());
            switch (smsg)
            {
                case Server.Handshake_BigEndian:
                    break;
                case Server.Handshake_LittleEndian:
                    break;
            }
            /*
                    private function handshake():void {
                        var messageID:int;
                        var size:int;
                        var p:int;
			
                        messageID = socket.readInt();
			
                        switch (messageID) {
                            case IncomingMessageTypes.UNKNOWN_SERVER: //1886610802
                                Alert.show("Got MSG_TROPSER.  Don't know how to proceed.","Logon Error");
                                break;
                            case IncomingMessageTypes.LITTLE_ENDIAN_SERVER: // MSG_DIYIT
                                socket.endian = Endian.LITTLE_ENDIAN;
                                size = socket.readInt();
                                p = socket.readInt();
                                logOn(size, p);
                                break;
                            case IncomingMessageTypes.BIG_ENDIAN_SERVER: // MSG_TIYID
                                socket.endian = Endian.BIG_ENDIAN;
                                size = socket.readInt();
                                p = socket.readInt();
                                logOn(size, p);
                                break;
                            default:
                                trace("Unexpected MessageID while logging on: " + messageID.toString());
                                break;
                        }
                    }
            */
        }

        void Flush()
        {
            /*
                int ret = 0;
                int sofar = 0;
                int max = sendQueue.size();

                if(sendQueue.size() == 0)
                    return;

                unsigned char * buf = new unsigned char[max];
                while(sendQueue.size() > 0){
                    buf[sofar++] = sendQueue.front();
                    sendQueue.pop_front();
                }

                sofar = 0;
                while(sofar < max){
                    ret = send(sock, &buf[sofar], max - sofar, 0);
                    if(ret == -1){
                        printf("Send Error\n");
                        perror("send");
                        Disconnect();
                        break;
                    } else 
                        sofar += ret;
                }
                delete[] buf;
             */
        }
    }
}
