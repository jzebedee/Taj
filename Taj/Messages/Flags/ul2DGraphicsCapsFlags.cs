using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages.Flags
{
    /// <summary>
    /// ul2DGraphicsCaps allegedly indicates the client’s 2-D graphics capabilities. It and the flag bit values are completely unused on the server.
    /// </summary>
    [Flags]
    public enum ul2DGraphicsCapsFlags : uint
    {
        NONE    = 0x00000000,
        GIF87   = 0x00000001,
        GIF89a  = 0x00000002,
        JPG     = 0x00000004,
        TIFF    = 0x00000008,
        TARGA   = 0x00000010,
        BMP     = 0x00000020,
        PCT     = 0x00000040,
    }
}
