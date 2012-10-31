using System.Diagnostics;
using Taj.Messages.Structures;

namespace Taj.Messages
{
    /// <summary>
    /// This message is sent from the server to the client to describe a new user who has 
    /// entered the room.
    /// </summary>
    public class MH_UserNew : MessageHeader
    {
        public MH_UserNew(IPalaceConnection con, ClientMessage cmsg)
            : base(con, cmsg)
        {
            var userID = cmsg.refNum; //The refnum field contains the UserID of the new user.
            var userRec = Reader.ReadStruct<UserRec>(); //The msg field contains a UserRec struct describing the new user:
            var user = Palace.GetUserByID(userID, true);

            userRec.Populate(user);
        }
    }
}