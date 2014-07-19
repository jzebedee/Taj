using System.Diagnostics;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    /// <summary>
    /// This message is sent from the server to the client to inform the client that a
    /// (different) user has left the room that the client is in.
    /// </summary>
    public class MH_UserExit : MessageReader
    {
        public MH_UserExit(ClientMessage cmsg)
            : base(cmsg)
        {
            ExitingUserID = cmsg.refNum; //The refnum field contains the UserID of the user who left the room.

            Debug.WriteLine("User left current room: " + ExitingUserID);
        }

        public int ExitingUserID { get; private set; }
    }
}