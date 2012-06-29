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
                if (eventType == Server.Handshake_BigEndian)
                    endianness = MiscUtil.Conversion.EndianBitConverter.Big;
                else if (eventType == Server.Handshake_LittleEndian)
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
            /*
		private function logOn(size:int, referenceId:int):void {
			var i:int;
			
//			trace("Logging on.  a: " + size + " - b: " + referenceId);
			// a is validation
			currentRoom.selfUserId = id = referenceId;


			// LOGON
			socket.writeInt(OutgoingMessageTypes.LOGON);
			socket.writeInt(128); // struct AuxRegistrationRec is 128 bytes
			socket.writeInt(0); // RefNum unused in LOGON message

			// regCode crc
			socket.writeInt(regCRC);  // Guest regCode crc
			
			// regCode counter
			socket.writeInt(regCounter);  // Guest regCode counter

			// Username has to be Windows-1252 and up to 31 characters
			if (userName.length > 31) {
				userName = userName.slice(0,31);
			}
			socket.writeByte(userName.length);
			socket.writeMultiByte(userName, 'Windows-1252');
			i = 31 - (userName.length);
			for(; i > 0; i--) { 
				socket.writeByte(0);
			}

			for (i=0; i < 32; i ++) {
				socket.writeByte(0);
			}			
	
			// auxFlags
			socket.writeInt(AUXFLAGS_AUTHENTICATE | AUXFLAGS_WIN32);

			// puidCtr
			socket.writeInt(puidCounter);
	
        	// puidCRC
			socket.writeInt(puidCRC);
	
        	// demoElapsed - no longer used
			socket.writeInt(0);
	
        	// totalElapsed - no longer used
			socket.writeInt(0);
	
        	// demoLimit - no longer used
			socket.writeInt(0);
        
			// desired room id
			socket.writeShort(initialRoom);

			// Protocol spec lists these as reserved, and says there shouldn't
			// be anything put in them... but the server records these 6 bytes
			// in the log file.  So I'll exploit that.
			socket.writeMultiByte("OPNPAL", "iso-8859-1");
	
			// ulRequestedProtocolVersion -- ignored on server
	        socket.writeInt(0);

			// ulUploadCaps
    	    socket.writeInt(
    	    	ULCAPS_ASSETS_PALACE  // This is a lie... for now
    	    );

        	// ulDownloadCaps
        	// We have to lie about our capabilities so that servers don't
        	// reject OpenPalace as a Hacked client.
	        socket.writeInt(
	        	DLCAPS_ASSETS_PALACE |
	        	DLCAPS_FILES_PALACE |  // This is a lie...
	        	DLCAPS_FILES_HTTPSRVR
	        );

        	// ul2DEngineCaps -- Unused
        	socket.writeInt(0);

        	// ul2dGraphicsCaps -- Unused
        	socket.writeInt(0);

			// ul3DEngineCaps -- Unused
        	socket.writeInt(0);
        	
			socket.flush();
			
			state = STATE_READY;
			connecting = false;
			dispatchEvent(new PalaceEvent(PalaceEvent.CONNECT_COMPLETE));
		}
            */

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
