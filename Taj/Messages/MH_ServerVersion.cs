using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public class MH_ServerVersion : IMessageHandler
    {
        Version rVer;

        public MH_ServerVersion(ClientMessage cmsg)
        {
            short
                refVerLo = (short)(cmsg.refNum),
                refVerHi = (short)(cmsg.refNum >> 16);

            rVer = new Version(refVerHi, refVerLo);
        }

        public Version Version
        {
            get
            {
                return rVer;
            }
        }

        public void Write(EndianBinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
