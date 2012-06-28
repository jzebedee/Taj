using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Net.Sockets;

namespace Taj
{
    public class Palace : IDisposable
    {
        readonly TcpClient connection;
        readonly NetworkStream stream;

        public Palace(Uri target)
        {
            connection = new TcpClient(target.Host, target.Port);
            stream = connection.GetStream();
            Handshake();
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
            stream.Dispose();

            if (disposing)
                GC.SuppressFinalize(this);
        }

        void Handshake()
        {

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
