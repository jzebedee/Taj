using MiscUtil.Conversion;
using MiscUtil.IO;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    public abstract class MessageReader
    {
        public static bool LittleEndian = BitConverter.IsLittleEndian;
        internal static EndianBitConverter Endianness
        {
            get
            {
                if (LittleEndian)
                    return EndianBitConverter.Little;
                return EndianBitConverter.Big;
            }
        }

        private readonly MemoryStream _ms;

        protected MessageReader()
        {
        }
        protected MessageReader(byte[] backing)
            : this()
        {
            if (backing != null)
            {
                this._ms = new MemoryStream(backing);
                this.Reader = new EndianBinaryReader(Endianness, _ms); 
            }
        }
        protected MessageReader(ClientMessage header, byte[] backing = null)
            : this(backing)
        {
            this.Header = header;
        }

        protected readonly ClientMessage Header;
        protected readonly EndianBinaryReader Reader;
    }
}
