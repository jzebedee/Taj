using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Messages.Flags
{
    /// <summary>
    /// scriptEventMask is a set of bit flags that encode what events a hotspot responds to
    /// </summary>
    [Flags]
    public enum HotspotScriptEventMask
    {
        Select = 0x00000001,
        Lock = 0x00000002,
        Unlock = 0x00000004,
        Hide = 0x00000008,
        Show = 0x00000010,
        Startup = 0x00000020,
        Alarm = 0x00000040,
        Custom = 0x00000080,
        InChat = 0x00000100,
        PropChange = 0x00000200,
        Enter = 0x00000400,
        Leave = 0x00000800,
        OutChat = 0x00001000,
        SignOn = 0x00002000,
        SignOff = 0x00004000,
        Macro0 = 0x00008000,
        Macro1 = 0x00010000,
        Macro2 = 0x00020000,
        Macro3 = 0x00040000,
        Macro4 = 0x00080000,
        Macro5 = 0x00100000,
        Macro6 = 0x00200000,
        Macro7 = 0x00400000,
        Macro8 = 0x00800000,
        Macro9 = 0x01000000,
    }
}
