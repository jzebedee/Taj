using System.Diagnostics;
using Palace.Messages.Structures;

namespace Palace.Messages
{
    /// <summary>
    /// This message is sent from the server to the client to describe a new user who has 
    /// entered the room.
    /// </summary>
    public class MH_UserNew : MessageReader
    {
        public MH_UserNew(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            NewUserID = cmsg.refNum; //The refnum field contains the UserID of the new user.
            NewUserRecord = Reader.ReadStruct<UserRec>(); //The msg field contains a UserRec struct describing the new user

            Debug.WriteLine("New user: " + NewUserID);
        }

        public int NewUserID { get; private set; }
        public UserRec NewUserRecord { get; private set; }
    }
}