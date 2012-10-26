using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages.Flags
{
    [Flags]
    public enum ServerOptionsFlags : uint
    {
        SaveSessionKeys     = 0x00000001, //server logs regcodes of users (obsolete)
        PasswordSecurity    = 0x00000002, //you need a password to use this server
        ChatLog             = 0x00000004, //server logs all chat
        NoWhisper           = 0x00000008, //whisper command disabled
        AllowDemoMembers    = 0x00000010, //obsolete
        Authenticate        = 0x00000020, //
        PoundProtect        = 0x00000040, //server employs heuristics to evade hackers
        SortOptions         = 0x00000080, //
        AuthTrackLogoff     = 0x00000100, //server logs logoffs 
        JavaSecure          = 0x00000200, //server supports Java client’s auth. scheme
    }
}
