using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palace.Messages.Flags
{
    [Flags]
    public enum UserFlags
    {
        SuperUser		= 0x0001, //wizard
        God		    	= 0x0002, //total wizard
        Kill			= 0x0004, //server should drop user at first opportunity
        Guest			= 0x0008, //user is a guest (i.e., no registration code)
        Banished		= 0x0010, //redundant with U_Kill, shouldn’t be used
        Penalized		= 0x0020, //historical artifact, shouldn’t be used
        CommError		= 0x0040, //comm error, drop at first opportunity
        Gag			    = 0x0080, //not allowed to speak
        Pin			    = 0x0100, //stuck in corner and not allowed to move
        Hide			= 0x0200, //doesn’t appear on user list
        RejectESP		= 0x0400, //not accepting whisper from outside room
        RejectPrivate	= 0x0800, //not accepting whisper from inside room
        PropGag		    = 0x1000, //not allowed to have props
    }
}
