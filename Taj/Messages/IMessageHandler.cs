using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.IO;

namespace Taj.Messages
{
    public interface IMessageHandler
    {
        //void Read(EndianBinaryReader reader);
        void Write(EndianBinaryWriter writer);
    }
}
