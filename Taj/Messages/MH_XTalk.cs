using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public class MH_XTalk : IMessageHandler
    {
        public MH_XTalk(string msg)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg;
        }
        public MH_XTalk(ClientMessage cmsg, EndianBinaryReader reader)
        {
            var len = reader.ReadInt16();
            byte[] xmsg = reader.ReadBytes(len - 3);
            reader.ReadByte();
            Text = Decrypt(xmsg);
        }

        private string Decrypt(byte[] xmsg)
        {
            return new PalaceEncryption().Decrypt(xmsg);
        }
        private byte[] Encrypt(string msg)
        {
            return new PalaceEncryption().Encrypt(msg);
        }

        public string Text { get; private set; }

        public void Write(EndianBinaryWriter writer)
        {
            var xmsg = Encrypt(Text);

            writer.WriteStruct(new ClientMessage
            {
                eventType = MessageTypes.MSG_XTALK,
                length = xmsg.Length + sizeof(short),
                refNum = 0, //TODO: set refnum to userid
            });
            writer.Write((short)xmsg.Length);
            writer.Write(xmsg);
            writer.Write((byte)0);
            writer.Flush();
        }
    }
}
