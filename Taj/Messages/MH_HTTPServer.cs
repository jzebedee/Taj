using System;

namespace Taj.Messages
{
    public class MH_HTTPServer : MessageHeader
    {
        public readonly Uri Location;

        public MH_HTTPServer(PalaceConnection con) : base(con)
        {
            string uri_string = Reader.ReadCString();
            Location = new Uri(uri_string);
        }
    }
}