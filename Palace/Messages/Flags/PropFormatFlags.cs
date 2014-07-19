using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Messages.Flags
{
    [Flags]
    public enum PropFormatFlags
    {
        HEAD = 0x02,
        GHOST = 0x04,
        RARE = 0x08,
        ANIMATE = 0x10,
        BOUNCE = 0x20,

        _8BIT = 0x00,
        _16BIT = 0xff80, //65535 - 127
        _20BIT = 0x40,
        _32BIT = 0x100,
        _S20BIT = 0x200,
    }
}
