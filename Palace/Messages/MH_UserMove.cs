using System.Diagnostics;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    /// <summary>
    /// This message is used to screen location of a user's avatar. A client sends a 
    /// MSG_USERMOVE message to the server requesting the change. If the operation is 
    /// successful, the server sends a matching MSG_USERMOVE message to the other 
    /// clients in the room, informing them of the event.
    /// </summary>
    public class MH_UserMove : MessageHeader
    {
        public MH_UserMove(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            var user = Palace.GetUserByID(cmsg.refNum); //The refnum field contains the UserID of the user who is moving

            var point = Reader.ReadStruct<Point>(); //The msg field indicates what the new position is supposed to be
            user.X = point.x;
            user.Y = point.y;

            Debug.WriteLine("{0} moved to ({1},{2})", user, point.x, point.y);
        }
    }
}