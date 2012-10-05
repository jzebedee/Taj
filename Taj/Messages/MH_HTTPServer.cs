using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;
using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_HTTPServer : MessageHeader
    {
        public readonly Uri Location;

        public MH_HTTPServer(PalaceConnection con) : base(con)
        {
            var uri_string = Reader.ReadCString();
            Location = new Uri(uri_string);
        }
    }
}