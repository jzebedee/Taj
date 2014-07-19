using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palace.Messages.Flags
{
    /// <summary>
    /// RoomFlags are various bit flags describing attributes of the room
    /// </summary>
    [Flags]
    public enum RoomFlags : uint
    {
        /// <summary>
        /// Non-owner can’t change (member created room)
        /// </summary>
        AuthorLocked	= 0x0001,
        /// <summary>
        /// Not in room list, in user list as “private”
        /// </summary>
        Private			= 0x0002,
        /// <summary>
        /// Disables drawing commands in room
        /// </summary>
        NoPainting		= 0x0004,
        /// <summary>
        /// No entry permitted (those in can stay)
        /// </summary>
        Closed			= 0x0008,
        /// <summary>
        /// Client disables cyborg.ipt scripts in room
        /// </summary>
        CyborgFreeZone	= 0x0010,
        /// <summary>
        /// Room doesn’t show up in goto list
        /// </summary>
        Hidden			= 0x0020,
        /// <summary>
        /// Guest users not permitted in room
        /// </summary>
        NoGuests		= 0x0040,
        /// <summary>
        /// Only wizards permitted in room
        /// </summary>
        WizardsOnly		= 0x0080,
        /// <summary>
        /// One of the rooms in which new users arrive
        /// </summary>
        DropZone		= 0x0100,
    }
}
