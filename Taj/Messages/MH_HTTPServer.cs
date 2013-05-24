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
            Debug.WriteLine("HTTPServer: " + uri_string);

            Palace.HTTPServer = new Uri(uri_string);
        }
    }
}