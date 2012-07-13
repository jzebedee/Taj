using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Taj.Messages
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ClientMsg_talk
    {
        public ClientMsg_talk(string message)
        {
            if (message.Length > 255)
                message = message.Substring(0, 255);

            //text = message;
        }

        //[MarshalAs(UnmanagedType.)]
        //public string text;
    }
}
