using System.Diagnostics;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    /// <summary>
    /// This message is sent from the server to the client to inform the client that a
    /// (different) user has left the room that the client is in.
    /// </summary>
    public class MH_UserExit : MessageHeader
    {
        public MH_UserExit(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            var userID = cmsg.refNum; //The refnum field contains the UserID of the user who left the room.
            var user = Palace.GetUserByID(userID, true);

            Debug.WriteLine("User left current room: " + user);

            Palace.RemoveUser(user);
        }
    }
}