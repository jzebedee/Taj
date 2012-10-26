using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages.Flags
{
    /// <summary>
    /// auxFlags indicate various attributes of the user’s machine
    /// </summary>
    [Flags]
    public enum AuxFlags : uint
    {
        UnknownMach     = 0,
        Mac68k          = 1,
        MacPPC          = 2,
        Win16           = 3,
        Win32           = 4,
        Java            = 5,
        OSMask          = 0x0000000F,
        Authenticate    = 0x80000000
    }
}
