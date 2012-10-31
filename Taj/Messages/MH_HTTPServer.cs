using System;
using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_HTTPServer : MessageHeader
    {
        public MH_HTTPServer(PalaceConnection con)
            : base(con)
        {
            string uri_string = Reader.ReadCString();
            Palace.HTTPServer = new Uri(uri_string);

            Debug.WriteLine("HTTPServer URI: " + uri_string);
        }
    }
}