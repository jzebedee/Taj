using System.Diagnostics;
using Palace.Messages.Structures;
namespace Palace.Messages
{
    public class MH_Logoff : MessageWriter
    {
        public MH_Logoff(ClientMessage cmsg, byte[] backing)
            : base(cmsg, backing)
        {
            LoggedOffID = cmsg.refNum;
            NewUserCount = Reader.ReadInt32();

            Debug.WriteLine("Lost user {0}", cmsg.refNum);
            Debug.WriteLine("New user count: {0}", NewUserCount);
        }
        public MH_Logoff()
        {

        }

        public int LoggedOffID { get; private set; }
        public int NewUserCount { get; private set; }

        #region IOutgoingMessage Members

        public override byte[] Write()
        {
            Writer.WriteStruct(new ClientMessage
                                   {
                                       eventType = MessageTypes.LOGOFF,
                                       length = 0,
                                       refNum = 0, //intentional
                                   });
            Writer.Flush();

            return base.Write();
        }

        #endregion
    }
}