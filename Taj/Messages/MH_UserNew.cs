using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;
using System.Diagnostics;

namespace Taj.Messages
{
    public class MH_UserNew
    {
        public MH_UserNew(ClientMessage cmsg, EndianBinaryReader reader)
        {
            Debug.WriteLine("MH_UserNew is unimplemented, and skipping itself ahead.");
            reader.ReadBytes(cmsg.length);
        }
    }
}