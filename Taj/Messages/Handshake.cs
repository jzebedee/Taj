using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages
{
    public class Handshake : ClientMsg
    {
        public Handshake(BinaryReader br) : base(br)
        {
            var ServerType = (Server)Enum.Parse(typeof(Server), eventType.ToString());
            switch (ServerType)
            {
                case Server.Handshake_BigEndian:
                    break;
                case Server.Handshake_LittleEndian:
                    break;
                default:
                    throw new NotImplementedException(eventType);
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
    }
}
