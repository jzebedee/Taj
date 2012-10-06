using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taj.Messages
{
    [Flags]
    public enum ServerPermissions
    {
        AllowGuests        = 0x0001, //guests may use this server
        AllowCyborgs       = 0x0002, //clients can use cyborg.ipt scripts
        AllowPainting      = 0x0004, //clients may issue draw commands
        AllowCustomProps   = 0x0008, //clients may select custom props
        AllowWizards       = 0x0010, //wizards can use this server
        WizardsMayKill     = 0x0020, //wizards can kick off users
        WizardsMayAuthor   = 0x0040, //wizards can create rooms
        PlayersMayKill     = 0x0080, //normal users can kick each other off
        CyborgsMayKill     = 0x0100, //scripts can kick off users
        DeathPenalty       = 0x0200, //
        PurgeInactiveProps = 0x0400, //server discards unused props
        KillFlooders       = 0x0800, //users dropped if they do too much too fast
        NoSpoofing         = 0x1000, //command to speak as another is disabled
        MemberCreatedRooms = 0x2000, //users can create rooms
    }
}
