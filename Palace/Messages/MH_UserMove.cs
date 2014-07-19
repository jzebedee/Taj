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
    public class MH_UserMove : MessageReader
    {
        public MH_UserMove(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            MovedUserID = cmsg.refNum; //The refnum field contains the UserID of the user who is moving
            NewPosition = Reader.ReadStruct<Point>(); //The msg field indicates what the new position is supposed to be

            Debug.WriteLine("{0} moved to ({1},{2})", MovedUserID, NewPosition.x, NewPosition.y);
        }

        public int MovedUserID { get; private set; }
        public Point NewPosition { get; private set; }
    }
}