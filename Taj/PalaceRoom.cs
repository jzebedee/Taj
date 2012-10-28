using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taj.Messages.Flags;

namespace Taj
{
    public class PalaceRoom
    {
        public Palace Host { get; protected set; }
        /// <summary>
        /// Visible identifier of the room on a palace
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Numeric identifier of the room on a palace
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Flags describing attributes of the room
        /// </summary>
        public RoomFlags Flags { get; set; }
        /// <summary>
        /// One of a preset number of client-defined avatar
        /// appearances that should be displayed for the avatar
        /// when it is not showing a prop instead.
        /// </summary>
        public int FacesID { get; set; }



        public PalaceRoom(Palace host)
        {
            Host = host;
        }

        public override string ToString()
        {
            return string.Format("`{0}` ({1})", Name, ID);
        }
    }
}
