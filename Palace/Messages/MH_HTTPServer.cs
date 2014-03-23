using System;
using System.Diagnostics;

namespace Palace.Messages
{
    public class MH_HTTPServer : MessageHeader
    {
        public MH_HTTPServer(IPalaceConnection con)
            : base(con)
        {
            string uri_string = Reader.ReadCString();
            Debug.WriteLine("HTTPServer: " + uri_string);

            Palace.HTTPServer = uri_string;
        }
    }
}