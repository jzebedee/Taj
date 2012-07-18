using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public class ClientMsg_talk : FormattedMessage
    {
        public ClientMsg_talk(EndianBinaryReader br)
        {
            var msg = new ClientMsg(br);

            var sb = new StringBuilder();
            for (int i = 0; i < msg.length; i++)
                sb.Append(br.ReadByte());

            Text = sb.ToString();
        }

        public string Text { get; set; }
    }
}