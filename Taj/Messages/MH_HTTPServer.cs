using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;
using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_HTTPServer
    {
        public readonly Uri Location;

        public MH_HTTPServer(EndianBinaryReader reader)
        {
            var uri_string = reader.ReadCString();
            Location = new Uri(uri_string);
        }
    }
}