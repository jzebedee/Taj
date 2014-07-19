using MiscUtil.IO;
using Palace.Messages.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Messages
{
    public abstract class MessageWriter : MessageReader
    {
        private readonly MemoryStream _ms = new MemoryStream();

        protected MessageWriter()
        {
            Writer = new EndianBinaryWriter(MessageReader.Endianness, _ms);
        }
        protected MessageWriter(ClientMessage header, byte[] backing)
            : base(header, backing)
        {
            Writer = new EndianBinaryWriter(MessageReader.Endianness, _ms);
        }

        protected readonly EndianBinaryWriter Writer;

        public virtual byte[] Write()
        {
            return _ms.ToArray();
        }
    }
}