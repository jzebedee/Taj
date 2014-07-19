using System;
using System.Diagnostics;

namespace Palace.Messages
{
    public class MH_HTTPServer : MessageReader
    {
        public MH_HTTPServer(byte[] backing)
            : base(backing)
        {
            HTTPServer = Reader.ReadCString();
            Debug.WriteLine("HTTPServer: " + HTTPServer);
        }

        public string HTTPServer { get; private set; }
    }
}