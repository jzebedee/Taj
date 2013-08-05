using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palace.Messages.Flags
{
    /// <summary>
    /// ul3DEngineCaps allegedly indicates the client’s 3-D graphics capabilities. It and the flag bit values are completely unused on the server.
    /// </summary>
    [Flags]
    public enum ul3DEngineCapsFlags : uint
    {
        NONE    = 0x00000000,
        VRML1   = 0x00000001,
        VRML2   = 0x00000002,
    }
}
