using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages.Flags
{
    /// <summary>
    /// ul2DEngineCaps allegedly indicates the client’s 2-D display engine. It and the flag bit values are completely unused on the server.
    /// </summary>
    [Flags]
    public enum ul2DEngineCapsFlags : uint
    {
        NONE        = 0x00000000,
        PALACE      = 0x00000001,
        DOUBLEBYTE  = 0x00000002,
    }
}
