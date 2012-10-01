using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public class MH_Talk : IMessageHandler
    {
        public MH_Talk(string msg)
        {
            if (msg.Length > 255)
                msg = msg.Substring(0, 255);

            Text = msg;
        }
        public MH_Talk(EndianBinaryReader reader)
        {
            Text = reader.ReadCString();
        }

        public string Text { get; private set; }

        public void Write(EndianBinaryWriter writer)
        {
            writer.WriteStruct(new ClientMessage
            {
                eventType = MessageTypes.Talk,
                length = Text.Length+1,
                refNum = 0,
            });
            writer.Write(Encoding.GetEncoding("Windows-1252").GetBytes(Text + "\0"));
            writer.Flush();
        }
    }
}
